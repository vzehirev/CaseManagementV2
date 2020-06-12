using CaseManagement.Data;
using CaseManagement.Models;
using CaseManagement.Models.CaseModels;
using CaseManagement.Models.TaskModels;
using CaseManagement.Services.Cases;
using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.CaseStatuses;
using CaseManagement.ViewModels.CaseType;
using CaseManagement.ViewModels.QueueStatus;
using CaseManagement.ViewModels.Tasks;
using CaseManagement.ViewModels.Tasks.Create;
using CaseManagement.ViewModels.WaitingReason;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.Tasks
{
    public class TasksService : ITasksService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICasesService casesService;

        public TasksService(ApplicationDbContext dbContext, ICasesService casesService)
        {
            this.dbContext = dbContext;
            this.casesService = casesService;
        }

        public async Task<int> CreateTaskAsync(TaskCreateViewModel viewModel, string userId)
        {
            CaseTask taskToAdd = new CaseTask
            {
                ReportedAt = viewModel.ReportedAt,
                LastUpdatedUtc = DateTime.UtcNow,
                UserId = userId,
                CaseId = viewModel.CaseId,
                TypeId = viewModel.SelectedTypeId,
                StatusId = viewModel.SelectedStatusId,
                AssignedProcessor = viewModel.AssignedProcessor,
                Notes = viewModel.Notes,
                PriorityId = viewModel.SelectedPriorityId,
                Queue = viewModel.Queue,
                QueueStatusId = viewModel.SelectedQueueStatusId,
                ResumeAt = viewModel.ResumeAt,
                Subject = viewModel.Subject,
                WaitingReasonId = viewModel.SelectedWaitingReasonId,
            };

            dbContext.Tasks.Add(taskToAdd);
            int result = await dbContext.SaveChangesAsync();

            await casesService.UpdateCaseAsync(taskToAdd);

            return result;
        }

        public async Task<ICollection<Models.TaskModels.TaskStatus>> GetAllTaskStatusesAsync()
        {
            return await dbContext.TaskStatuses.ToArrayAsync();
        }

        public async Task<ICollection<TaskType>> GetAllTaskTypesAsync()
        {
            return await dbContext.TaskTypes.ToArrayAsync();
        }

        public async Task<ViewUpdateTaskIOModel> GetTaskByIdAsync(int id)
        {
            ViewUpdateTaskIOModel outputModel = await dbContext.Tasks
                .Select(t => new ViewUpdateTaskIOModel
                {
                    Id = t.Id,
                    CreatedOn = t.ReportedAt,
                    StatusId = t.Status.Id,
                    TypeId = t.Type.Id,
                    CaseId = t.CaseId,
                    TaskTypes = dbContext.TaskTypes.ToArray(),
                    TaskStatuses = dbContext.TaskStatuses.ToArray(),
                })
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            return outputModel;
        }

        public async Task<TaskCreateViewModel> GenerateTaskCreateViewModel(int caseId)
        {
            var result = await dbContext.Cases
                .Where(x => x.Id == caseId)
                .Select(x => new TaskCreateViewModel
                {
                    CaseNumber = x.Number,
                    SelectedPriorityId = x.PriorityId,
                    Subject = x.Subject,
                    SelectedStatusId = x.StatusId,
                    SelectedWaitingReasonId = x.WaitingReasonId,
                    SelectedTypeId = x.TypeId,
                    Queue = x.Queue,
                    SelectedQueueStatusId = x.QueueStatusId,
                    ReportedAt = x.ReportedAt,
                    ResumeAt = x.ResumeAt,
                    AssignedProcessor = x.AssignedProcessor,
                    Notes = x.Notes,
                    AllPriorities = dbContext.CasePriorities
                        .Where(x => x.CasePriorityName != "N/A")
                        .Select(x => new CasePriorityViewModel
                        {
                            Id = x.Id,
                            CasePriorityName = x.CasePriorityName,
                        }),
                    AllQueueStatuses = dbContext.QueueStatuses
                        .Select(x => new QueueStatusViewModel
                        {
                            Id = x.Id,
                            QueueStatusName = x.QueueStatusName,
                        }),
                    AllTypes = dbContext.CaseTypes
                        .Where(x => x.CaseTypeName != "N/A")
                        .Select(x => new CaseTypeViewModel
                        {
                            Id = x.Id,
                            CaseTypeName = x.CaseTypeName,
                        }),
                    AllStatuses = dbContext.CaseStatuses
                        .Where(x => x.CaseStatusName != "N/A")
                        .Select(x => new CaseStatusViewModel
                        {
                            Id = x.Id,
                            CaseStatusName = x.CaseStatusName,
                        }),
                    AllWaitingReasons = dbContext.WaitingReasons
                        .Select(x => new WaitingReasonViewModel
                        {
                            Id = x.Id,
                            WaitingReasonName = x.WaitingReasonName,
                        }),
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<int> UpdateTaskAsync(ViewUpdateTaskIOModel inputModel, string userId)
        {
            CaseTask taskRecordToUpdate = await dbContext.Tasks
                   .FirstOrDefaultAsync(t => t.Id == inputModel.Id);

            List<FieldModification> fieldModifications = new List<FieldModification>();

            if (taskRecordToUpdate.TypeId != inputModel.TypeId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Type",
                    OldValue = await dbContext.TaskTypes.Where(tt => tt.Id == taskRecordToUpdate.TypeId).Select(tt => tt.Type).FirstOrDefaultAsync(),
                    NewValue = await dbContext.TaskTypes.Where(tt => tt.Id == inputModel.TypeId).Select(tt => tt.Type).FirstOrDefaultAsync(),
                });
            }

            if (taskRecordToUpdate.StatusId != inputModel.StatusId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Status",
                    OldValue = await dbContext.TaskStatuses.Where(ts => ts.Id == taskRecordToUpdate.StatusId).Select(ts => ts.Status).FirstOrDefaultAsync(),
                    NewValue = await dbContext.TaskStatuses.Where(ts => ts.Id == inputModel.StatusId).Select(ts => ts.Status).FirstOrDefaultAsync(),
                });
            }

            taskRecordToUpdate.TypeId = inputModel.TypeId;
            taskRecordToUpdate.LastUpdatedUtc = DateTime.UtcNow;
            taskRecordToUpdate.StatusId = inputModel.StatusId;

            if (fieldModifications.Count > 0)
            {
                TaskModificationLogRecord modificationLogRecord = new TaskModificationLogRecord
                {
                    ModificationTime = DateTime.UtcNow,
                    UserId = userId,
                    TaskId = taskRecordToUpdate.Id,
                    ModifiedFields = fieldModifications,
                };

                dbContext.TaskModificationLogRecords.Add(modificationLogRecord);
            }

            dbContext.Tasks.Update(taskRecordToUpdate);

            int saveResult = await dbContext.SaveChangesAsync();

            return saveResult;
        }
    }
}
