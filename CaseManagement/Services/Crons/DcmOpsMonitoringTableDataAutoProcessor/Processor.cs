using CaseManagement.Data;
using CaseManagement.Helpers;
using CaseManagement.Models;
using CaseManagement.Models.CaseModels;
using CaseManagement.Models.TaskModels;
using CaseManagement.Services.Cases;
using CaseManagement.Services.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
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

        public Processor(ApplicationDbContext dbContext, ICasesService casesService)
        {
            this.dbContext = dbContext;
            this.casesService = casesService;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var lastProcessedRecord = await dbContext.Cases
                    .OrderBy(x => x.SNo)
                    .Select(x => x.SNo)
                    .LastOrDefaultAsync();

                var notProcessedRecords = await dbContext.DCMOpsMonitoring
                    .Where(x => x.SNo > lastProcessedRecord)
                    .OrderBy(x => x.SNo)
                    .ToArrayAsync();

                var casePriorities = await dbContext.CasePriorities.ToDictionaryAsync(x => x.CasePriorityName.ToLower(), x => x.Id);
                var caseStatuses = await dbContext.CaseStatuses.ToDictionaryAsync(x => x.CaseStatusName.ToLower(), x => x.Id);
                var caseTypes = await dbContext.CaseTypes.ToDictionaryAsync(x => x.CaseTypeName.ToLower(), x => x.Id);
                var caseWaitingReasons = await dbContext.WaitingReasons.ToDictionaryAsync(x => x.WaitingReasonName.ToLower(), x => x.Id);
                var caseQueueStatuses = await dbContext.QueueStatuses.ToDictionaryAsync(x => x.QueueStatusName.ToLower(), x => x.Id);

                for (int i = 0; i < notProcessedRecords.Length; i++)
                {
                    var record = notProcessedRecords[i];
                    try
                    {
                        var alreadyExistingCase = await dbContext.Cases
                            .FirstOrDefaultAsync(x => x.Number == record.TicketID.ToString());

                        if (alreadyExistingCase != null)
                        {
                            var taskToAdd = new CaseTask();

                            taskToAdd.CaseId = alreadyExistingCase.Id;
                            taskToAdd.AssignedProcessor = NullOrTheStringValueTrimmed(record.Assigned);
                            taskToAdd.LastUpdatedUtc = record.UpdatedDate == null ? DateTime.UtcNow : record.UpdatedDate;
                            taskToAdd.Notes = NullOrTheStringValueTrimmed(record.Notes);
                            taskToAdd.Queue = NullOrTheStringValueTrimmed(record.Queue);
                            taskToAdd.ResumeAt = record.Resumeat;
                            taskToAdd.Subject = NullOrTheStringValueTrimmed(record.Subject);

                            if (record.Priority != null && casePriorities.ContainsKey(record.Priority.Trim().ToLower()))
                            {
                                taskToAdd.PriorityId = casePriorities[record.Priority.Trim().ToLower()];
                            }
                            else
                            {
                                taskToAdd.PriorityId = casePriorities["n/a"];
                            }

                            if (record.Status != null && caseStatuses.ContainsKey(record.Status.Trim().ToLower()))
                            {
                                taskToAdd.StatusId = caseStatuses[record.Status.Trim().ToLower()];
                            }
                            else
                            {
                                taskToAdd.StatusId = caseStatuses["n/a"];
                            }

                            if (record.TicketType != null && caseTypes.ContainsKey(record.TicketType.Trim().ToLower()))
                            {
                                taskToAdd.TypeId = caseTypes[record.TicketType.Trim().ToLower()];
                            }
                            else
                            {
                                taskToAdd.TypeId = caseTypes["n/a"];
                            }

                            if (record.WaitingReason != null && caseWaitingReasons.ContainsKey(record.WaitingReason.Trim().ToLower()))
                            {
                                taskToAdd.WaitingReasonId = caseWaitingReasons[record.WaitingReason.Trim().ToLower()];
                            }

                            if (record.Qstatus != null && caseQueueStatuses.ContainsKey(record.Qstatus.Trim().ToLower()))
                            {
                                taskToAdd.QueueStatusId = caseQueueStatuses[record.Qstatus.Trim().ToLower()];
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
                        else
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


                            if (record.Priority != null && casePriorities.ContainsKey(record.Priority.Trim().ToLower()))
                            {
                                caseToAdd.PriorityId = casePriorities[record.Priority.Trim().ToLower()];
                            }
                            else
                            {
                                caseToAdd.PriorityId = casePriorities["n/a"];
                            }

                            if (record.Status != null && caseStatuses.ContainsKey(record.Status.Trim().ToLower()))
                            {
                                caseToAdd.StatusId = caseStatuses[record.Status.Trim().ToLower()];
                            }
                            else
                            {
                                caseToAdd.StatusId = caseStatuses["n/a"];
                            }

                            if (record.TicketType != null && caseTypes.ContainsKey(record.TicketType.Trim().ToLower()))
                            {
                                caseToAdd.TypeId = caseTypes[record.TicketType.Trim().ToLower()];
                            }
                            else
                            {
                                caseToAdd.TypeId = caseTypes["n/a"];
                            }

                            if (record.WaitingReason != null && caseWaitingReasons.ContainsKey(record.WaitingReason.Trim().ToLower()))
                            {
                                caseToAdd.WaitingReasonId = caseWaitingReasons[record.WaitingReason.Trim().ToLower()];
                            }

                            if (record.Qstatus != null && caseQueueStatuses.ContainsKey(record.Qstatus.Trim().ToLower()))
                            {
                                caseToAdd.QueueStatusId = caseQueueStatuses[record.Qstatus.Trim().ToLower()];
                            }

                            dbContext.Cases.Add(caseToAdd);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = new DcmOpsMonitoringTableProcessorError();

                        error.DateAndTime = DateTime.UtcNow;
                        error.ErrorMessage = $"Record SNo: {record.SNo}; Exception: {ex.Message}; Inner exception: {ex.InnerException}";

                        foreach (var entry in dbContext.ChangeTracker.Entries())
                        {
                            entry.State = EntityState.Detached;
                        }

                        dbContext.DcmOpsMonitoringTableProcessorErrors.Add(error);
                        await dbContext.SaveChangesAsync();
                    }
                }
                await Task.Delay(GlobalConstants.DcmOpsMonitoringTableAutoProcessorInterval, stoppingToken);
            }
        }

        private string NullOrTheStringValueTrimmed(string value)
        {
            return value == null ? value : value.Trim();
        }
    }
}