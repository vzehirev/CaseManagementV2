using CaseManagement.Data;
using CaseManagement.Models;
using CaseManagement.Models.CaseModels;
using CaseManagement.Models.DCMOpsMonitoringTable;
using CaseManagement.Models.TaskModels;
using CaseManagement.Services.Cases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Crons.DcmOpsMonitoringTableDataAutoProcessor
{
    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class Processor : IScopedProcessingService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICasesService casesService;

        private IDictionary<string, int> casePriorities;
        private IDictionary<string, int> caseStatuses;
        private IDictionary<string, int> caseTypes;
        private IDictionary<string, int> caseWaitingReasons;
        private IDictionary<string, int> caseQueueStatuses;

        public Processor(ApplicationDbContext dbContext, ICasesService casesService)
        {
            this.dbContext = dbContext;
            this.casesService = casesService;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.casePriorities = await dbContext.CasePriorities.ToDictionaryAsync(x => x.CasePriorityName.ToLower(), x => x.Id);
                this.caseStatuses = await dbContext.CaseStatuses.ToDictionaryAsync(x => x.CaseStatusName.ToLower(), x => x.Id);
                this.caseTypes = await dbContext.CaseTypes.ToDictionaryAsync(x => x.CaseTypeName.ToLower(), x => x.Id);
                this.caseWaitingReasons = await dbContext.WaitingReasons.ToDictionaryAsync(x => x.WaitingReasonName.ToLower(), x => x.Id);
                this.caseQueueStatuses = await dbContext.QueueStatuses.ToDictionaryAsync(x => x.QueueStatusName.ToLower(), x => x.Id);

                var lastProcessedRecord = await dbContext.Cases
                    .OrderBy(x => x.SNo)
                    .Select(x => x.SNo)
                    .LastOrDefaultAsync();

                var notProcessedRecords = await dbContext.DCMOpsMonitoring
                    .Where(x => x.SNo > lastProcessedRecord)
                    .OrderBy(x => x.SNo)
                    .ToArrayAsync();

                for (int i = 0; i < notProcessedRecords.Length; i++)
                {
                    var record = notProcessedRecords[i];

                    try
                    {
                        var alreadyExistingCase = await dbContext.Cases
                            .FirstOrDefaultAsync(x => x.Number == record.TicketID.ToString());

                        if (alreadyExistingCase != null)
                        {
                            await this.addNewTask(record, alreadyExistingCase);
                        }
                        else
                        {
                            await this.addNewCase(record);
                        }
                    }
                    catch (Exception ex)
                    {
                        await this.logProcessingError(ex, record.SNo);
                    }
                }
                await Task.Delay(GlobalConstants.DcmOpsMonitoringTableAutoProcessorInterval, stoppingToken);
            }
        }

        private string NullOrTheStringValueTrimmed(string value)
        {
            return value == null ? value : value.Trim();
        }

        private async Task addNewCase(DCMOpsMonitoringRow record)
        {
            var caseToAdd = new Case();

            if (record.TicketID == null)
            {
                throw new Exception($"No case number found at record with SNo = {record.SNo}");
            }

            caseToAdd.Number = record.TicketID.ToString();
            caseToAdd.SNo = record.SNo;
            caseToAdd.ReportedAt = record.ReportedDate == null ? DateTime.UtcNow : record.ReportedDate.Value;
            caseToAdd.AssignedProcessor = NullOrTheStringValueTrimmed(record.Assigned);
            caseToAdd.LastUpdatedUtc = record.UpdatedDate == null ? DateTime.UtcNow : record.UpdatedDate;
            caseToAdd.Notes = NullOrTheStringValueTrimmed(record.Notes);
            caseToAdd.Queue = NullOrTheStringValueTrimmed(record.Queue);
            caseToAdd.ResumeAt = record.Resumeat;
            caseToAdd.Subject = NullOrTheStringValueTrimmed(record.Subject);


            if (record.Priority != null && this.casePriorities.ContainsKey(record.Priority.Trim().ToLower()))
            {
                caseToAdd.PriorityId = this.casePriorities[record.Priority.Trim().ToLower()];
            }
            else
            {
                caseToAdd.PriorityId = this.casePriorities["n/a"];
            }

            if (record.Status != null && this.caseStatuses.ContainsKey(record.Status.Trim().ToLower()))
            {
                caseToAdd.StatusId = this.caseStatuses[record.Status.Trim().ToLower()];
            }
            else
            {
                caseToAdd.StatusId = this.caseStatuses["n/a"];
            }

            if (record.TicketType != null && this.caseTypes.ContainsKey(record.TicketType.Trim().ToLower()))
            {
                caseToAdd.TypeId = this.caseTypes[record.TicketType.Trim().ToLower()];
            }
            else
            {
                caseToAdd.TypeId = this.caseTypes["n/a"];
            }

            if (record.WaitingReason != null && this.caseWaitingReasons.ContainsKey(record.WaitingReason.Trim().ToLower()))
            {
                caseToAdd.WaitingReasonId = this.caseWaitingReasons[record.WaitingReason.Trim().ToLower()];
            }

            if (record.Qstatus != null && this.caseQueueStatuses.ContainsKey(record.Qstatus.Trim().ToLower()))
            {
                caseToAdd.QueueStatusId = this.caseQueueStatuses[record.Qstatus.Trim().ToLower()];
            }

            dbContext.Cases.Add(caseToAdd);
            await dbContext.SaveChangesAsync();
        }

        private async Task addNewTask(DCMOpsMonitoringRow record, Case alreadyExistingCase)
        {

            var taskToAdd = new CaseTask();

            taskToAdd.CaseId = alreadyExistingCase.Id;
            taskToAdd.AssignedProcessor = NullOrTheStringValueTrimmed(record.Assigned);
            taskToAdd.LastUpdatedUtc = record.UpdatedDate == null ? DateTime.UtcNow : record.UpdatedDate;
            taskToAdd.Notes = NullOrTheStringValueTrimmed(record.Notes);
            taskToAdd.Queue = NullOrTheStringValueTrimmed(record.Queue);
            taskToAdd.ResumeAt = record.Resumeat;
            taskToAdd.Subject = NullOrTheStringValueTrimmed(record.Subject);

            if (record.Priority != null && this.casePriorities.ContainsKey(record.Priority.Trim().ToLower()))
            {
                taskToAdd.PriorityId = this.casePriorities[record.Priority.Trim().ToLower()];
            }
            else
            {
                taskToAdd.PriorityId = this.casePriorities["n/a"];
            }

            if (record.Status != null && this.caseStatuses.ContainsKey(record.Status.Trim().ToLower()))
            {
                taskToAdd.StatusId = this.caseStatuses[record.Status.Trim().ToLower()];
            }
            else
            {
                taskToAdd.StatusId = this.caseStatuses["n/a"];
            }

            if (record.TicketType != null && this.caseTypes.ContainsKey(record.TicketType.Trim().ToLower()))
            {
                taskToAdd.TypeId = this.caseTypes[record.TicketType.Trim().ToLower()];
            }
            else
            {
                taskToAdd.TypeId = this.caseTypes["n/a"];
            }

            if (record.WaitingReason != null && this.caseWaitingReasons.ContainsKey(record.WaitingReason.Trim().ToLower()))
            {
                taskToAdd.WaitingReasonId = this.caseWaitingReasons[record.WaitingReason.Trim().ToLower()];
            }

            if (record.Qstatus != null && this.caseQueueStatuses.ContainsKey(record.Qstatus.Trim().ToLower()))
            {
                taskToAdd.QueueStatusId = this.caseQueueStatuses[record.Qstatus.Trim().ToLower()];
            }

            dbContext.Tasks.Add(taskToAdd);
            var addTaskResult = await dbContext.SaveChangesAsync();

            if (addTaskResult > 0)
            {
                var updateCaseResult = await casesService.UpdateCaseAsync(taskToAdd);

                if (updateCaseResult > 0)
                {
                    alreadyExistingCase.SNo = record.SNo;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task logProcessingError(Exception ex, int recordSNo)
        {
            var error = new DcmOpsMonitoringTableProcessorError();

            error.DateAndTime = DateTime.UtcNow;
            error.ErrorMessage = $"Record SNo: {recordSNo}; Exception: {ex.Message}; Inner exception: {ex.InnerException}";

            this.detachAllCurrentlyTrackedDbEntities();

            dbContext.DcmOpsMonitoringTableProcessorErrors.Add(error);
            await dbContext.SaveChangesAsync();
        }

        private void detachAllCurrentlyTrackedDbEntities()
        {
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}