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
                    .OrderByDescending(x => x.SNo)
                    .Select(x => x.SNo)
                    .FirstOrDefaultAsync();


                var notProcessedRecords = await dbContext.DCMOpsMonitoring
                    .Where(x => x.SNo > lastProcessedRecord)
                    .ToArrayAsync();

                foreach (var record in notProcessedRecords)
                {
                    try
                    {
                        var alreadyExistingCase = await dbContext.Cases
                            .FirstOrDefaultAsync(x => x.Number.ToLower() == record.TicketID.ToString().ToLower());

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

                            if (record.Priority == null || dbContext.CasePriorities
                                .FirstOrDefault(x => x.CasePriorityName.ToLower() == record.Priority.Trim().ToLower()) == null)
                            {
                                taskToAdd.PriorityId = dbContext.CasePriorities.First(x => x.CasePriorityName == "N/A").Id;
                            }
                            else
                            {
                                taskToAdd.PriorityId = dbContext.CasePriorities
                                    .First(x => x.CasePriorityName.ToLower() == record.Priority.Trim().ToLower()).Id;
                            }

                            if (record.Status == null || dbContext.CaseStatuses
                                .FirstOrDefault(x => x.CaseStatusName.ToLower() == record.Status.Trim().ToLower()) == null)
                            {
                                taskToAdd.StatusId = dbContext.CaseStatuses.First(x => x.CaseStatusName == "N/A").Id;
                            }
                            else
                            {
                                taskToAdd.StatusId = dbContext.CaseStatuses
                                    .First(x => x.CaseStatusName.ToLower() == record.Status.Trim().ToLower()).Id;
                            }

                            if (record.TicketType == null || dbContext.CaseTypes
                                .FirstOrDefault(x => x.CaseTypeName.ToLower() == record.TicketType.Trim().ToLower()) == null)
                            {
                                taskToAdd.TypeId = dbContext.CaseTypes.First(x => x.CaseTypeName == "N/A").Id;
                            }
                            else
                            {
                                taskToAdd.TypeId = dbContext.CaseTypes
                                    .First(x => x.CaseTypeName.ToLower() == record.TicketType.Trim().ToLower()).Id;
                            }

                            if (record.WaitingReason != null)
                            {
                                taskToAdd.WaitingReasonId = dbContext.WaitingReasons
                                    .FirstOrDefault(x => x.WaitingReasonName.ToLower() == record.WaitingReason.Trim().ToLower())?.Id;
                            }

                            if (record.Qstatus != null)
                            {
                                taskToAdd.QueueStatusId = dbContext.QueueStatuses
                                    .FirstOrDefault(x => x.QueueStatusName.ToLower() == record.Qstatus.Trim().ToLower())?.Id;
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
                                throw new ArgumentException($"No case number found at record with SNo = {record.SNo}");
                            }

                            caseToAdd.Number = record.TicketID.ToString().Trim();
                            caseToAdd.SNo = record.SNo;
                            caseToAdd.ReportedAt = record.ReportedDate == null ? DateTime.UtcNow : record.ReportedDate.Value;
                            caseToAdd.AssignedProcessor = NullOrTheStringValueTrimmed(record.Assigned);
                            caseToAdd.LastUpdatedUtc = record.UpdatedDate == null ? DateTime.UtcNow : record.UpdatedDate;
                            caseToAdd.Notes = NullOrTheStringValueTrimmed(record.Notes);
                            caseToAdd.Queue = NullOrTheStringValueTrimmed(record.Queue);
                            caseToAdd.ResumeAt = record.Resumeat;
                            caseToAdd.Subject = NullOrTheStringValueTrimmed(record.Subject);

                            if (record.Priority == null || dbContext.CasePriorities
                                .FirstOrDefault(x => x.CasePriorityName.ToLower() == record.Priority.Trim().ToLower()) == null)
                            {
                                caseToAdd.PriorityId = dbContext.CasePriorities.First(x => x.CasePriorityName == "N/A").Id;
                            }
                            else
                            {
                                caseToAdd.PriorityId = dbContext.CasePriorities
                                    .First(x => x.CasePriorityName.ToLower() == record.Priority.Trim().ToLower()).Id;
                            }

                            if (record.Status == null || dbContext.CaseStatuses
                                .FirstOrDefault(x => x.CaseStatusName.ToLower() == record.Status.Trim().ToLower()) == null)
                            {
                                caseToAdd.StatusId = dbContext.CaseStatuses.First(x => x.CaseStatusName == "N/A").Id;
                            }
                            else
                            {
                                caseToAdd.StatusId = dbContext.CaseStatuses
                                    .First(x => x.CaseStatusName.ToLower() == record.Status.Trim().ToLower()).Id;
                            }

                            if (record.TicketType == null || dbContext.CaseTypes
                                .FirstOrDefault(x => x.CaseTypeName.ToLower() == record.TicketType.Trim().ToLower()) == null)
                            {
                                caseToAdd.TypeId = dbContext.CaseTypes.First(x => x.CaseTypeName == "N/A").Id;
                            }
                            else
                            {
                                caseToAdd.TypeId = dbContext.CaseTypes
                                    .First(x => x.CaseTypeName.ToLower() == record.TicketType.Trim().ToLower()).Id;
                            }

                            if (record.WaitingReason != null)
                            {
                                caseToAdd.WaitingReasonId = dbContext.WaitingReasons
                                    .FirstOrDefault(x => x.WaitingReasonName.ToLower() == record.WaitingReason.Trim().ToLower())?.Id;
                            }

                            if (record.Qstatus != null)
                            {
                                caseToAdd.QueueStatusId = dbContext.QueueStatuses
                                    .FirstOrDefault(x => x.QueueStatusName.ToLower() == record.Qstatus.Trim().ToLower())?.Id;
                            }

                            dbContext.Cases.Add(caseToAdd);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = new DcmOpsMonitoringTableProcessorError();

                        error.DateAndTime = DateTime.UtcNow;
                        error.ErrorMessage = ex.Message;

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
