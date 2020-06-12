using CaseManagement.Data;
using CaseManagement.Data.Extensions;
using CaseManagement.Models;
using CaseManagement.Models.CaseModels;
using CaseManagement.Models.TaskModels;
using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.CasePriorities.Output;
using CaseManagement.ViewModels.Cases;
using CaseManagement.ViewModels.Cases.Index;
using CaseManagement.ViewModels.Cases.Input;
using CaseManagement.ViewModels.Cases.Output;
using CaseManagement.ViewModels.Cases.ViewUpdate;
using CaseManagement.ViewModels.CaseStatuses;
using CaseManagement.ViewModels.CaseStatuses.Output;
using CaseManagement.ViewModels.CaseType;
using CaseManagement.ViewModels.QueueStatus;
using CaseManagement.ViewModels.Tasks.Output;
using CaseManagement.ViewModels.WaitingReason;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.Cases
{
    public class CasesService : ICasesService
    {
        private readonly ApplicationDbContext dbContext;

        public CasesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CreateCaseAsync(CreateCaseInputModel inputModel, string userId)
        {
            Case caseToAdd = new Case
            {
                Number = inputModel.Number,
                Subject = inputModel.Subject,
                Notes = inputModel.Description,
                ReportedAt = DateTime.UtcNow,
                UserId = userId,
                TypeId = inputModel.TypeId,
                StatusId = inputModel.StatusId,
                PriorityId = inputModel.PriorityId,
            };

            dbContext.Cases.Add(caseToAdd);
            int saveResult = await dbContext.SaveChangesAsync();

            if (saveResult > 0)
            {
                return caseToAdd.Id;
            }

            return saveResult;
        }

        public async Task<IEnumerable<CasePriorityViewModel>> GetAllCasePrioritiesAsync()
        {
            var result = await dbContext.CasePriorities
                .Select(x => new CasePriorityViewModel
                {
                    Id = x.Id,
                    CasePriorityName = x.CasePriorityName,
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<int> GetCasesCountAsync(IEnumerable<int> selectedStatuses, IEnumerable<int> selectedPriorities)
        {
            var result = await dbContext.Cases
                .Where(c => selectedStatuses.Contains(c.StatusId) && selectedPriorities.Contains(c.PriorityId))
                .CountAsync();

            return result;
        }

        public async Task<IEnumerable<CaseViewModel>> GetCasesAsync(int skip, int take,
            IEnumerable<int> selectedStatuses, IEnumerable<int> selectedPriorities,
            string orderBy)
        {
            var result = await dbContext.Cases
                    .Where(c => selectedStatuses.Contains(c.StatusId) && selectedPriorities.Contains(c.PriorityId))
                    .CustomCasesOrder(orderBy)
                    .Skip(skip)
                    .Take(take)
                    .Select(c => new CaseViewModel
                    {
                        Id = c.Id,
                        ReportedAt = c.ReportedAt,
                        Number = c.Number,
                        Status = c.Status.CaseStatusName,
                        Priority = c.Priority.CasePriorityName,
                        Subject = c.Subject,
                        AssignedProcessor = c.AssignedProcessor,
                        ChangedByUser = c.User == null ? null : (c.User.FullName ?? null),
                        LastUpdatedUtc = c.LastUpdatedUtc,
                        Notes = c.Notes,
                        ResumeAtUtc = c.ResumeAt,
                        WaitingReason = c.WaitingReason.WaitingReasonName,
                    })
                    .ToArrayAsync();

            return result;
        }

        public async Task<AllCasesOutputModel> GetCasesAsync(int skip, int take, string orderBy,
            IEnumerable<int> selectedStatuses, IEnumerable<int> selectedPriorities, string userId = null)
        {
            IQueryable<Case> query = dbContext.Cases.AsQueryable();

            if (userId != null)
            {
                query = query.Where(c => c.UserId == userId);
            }

            AllCasesOutputModel result = new AllCasesOutputModel
            {
                AllCases = await query
                    .Where(c => selectedStatuses.Contains(c.StatusId) && selectedPriorities.Contains(c.PriorityId))
                    .CountAsync(),
                Cases = await query
                    .Where(c => selectedStatuses.Contains(c.StatusId) && selectedPriorities.Contains(c.PriorityId))
                    .CustomCasesOrder(orderBy)
                    .Skip(skip)
                    .Take(take)
                    .Select(c => new CaseOutputModel
                    {
                        Id = c.Id,
                        Number = c.Number,
                        CreatedOn = c.ReportedAt,
                        Status = c.Status.CaseStatusName,
                        Priority = c.Priority.CasePriorityName,
                        Subject = c.Subject,
                        Agent = c.User.Email,
                    })
                    .ToArrayAsync(),
                AllAvailableCaseStatuses = await dbContext.CaseStatuses
                    .Select(cp => new CaseStatusOuputModel
                    {
                        Id = cp.Id,
                        Status = cp.CaseStatusName,
                    })
                    .ToArrayAsync(),
                AllAvailableCasePriorities = await dbContext.CasePriorities
                    .Select(cp => new CasePriorityOutputModel
                    {
                        Id = cp.Id,
                        Priority = cp.CasePriorityName,
                    })
                    .ToArrayAsync(),
            };

            return result;
        }

        public async Task<IEnumerable<Service>> GetAllCaseServicesAsync()
        {
            return await dbContext.Services.ToArrayAsync();
        }

        public async Task<IEnumerable<CaseStatusViewModel>> GetAllCaseStatusesAsync()
        {
            var result = await dbContext.CaseStatuses
                .Select(x => new CaseStatusViewModel
                {
                    Id = x.Id,
                    CaseStatusName = x.CaseStatusName,
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<CaseTypeViewModel>> GetAllCaseTypesAsync()
        {
            var result = await dbContext.CaseTypes
                .Select(x => new CaseTypeViewModel
                {
                    Id = x.Id,
                    CaseTypeName = x.CaseTypeName,
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<ViewUpdateCaseIOModel> GetCaseByIdAsync(int id, int skipTasks, int takeTasks)
        {
            ViewUpdateCaseIOModel outputModel = await dbContext.Cases
                .Select(c => new ViewUpdateCaseIOModel
                {
                    Id = c.Id,
                    Number = c.Number,
                    CreatedOn = c.ReportedAt,
                    TypeId = c.Type.Id,
                    StatusId = c.Status.Id,
                    PriorityId = c.Priority.Id,
                    Subject = c.Subject,
                    Description = c.Notes,
                    AllTasks = c.Tasks.Count(),
                    CaseStatuses = dbContext.CaseStatuses.ToArray(),
                    CasePriorities = dbContext.CasePriorities.ToArray(),
                    CaseTypes = dbContext.CaseTypes.Select(x => new CaseTypeViewModel { Id = x.Id, CaseTypeName = x.CaseTypeName })
                        .ToArray(),
                    CaseServices = dbContext.Services.ToArray(),
                })
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            outputModel.Tasks = await dbContext.Tasks
                .Where(t => t.CaseId == id)
                .OrderByDescending(t => t.ReportedAt)
                .Skip(skipTasks)
                .Take(takeTasks)
                .Select(t => new TaskOutputModel
                {
                    Id = t.Id,
                    CreatedOn = t.ReportedAt,
                    Type = t.Type.CaseTypeName,
                    Status = t.Status.CaseStatusName,
                    Agent = t.User.Email
                }).ToArrayAsync();

            return outputModel;
        }

        public async Task<CaseViewModel> GetCaseByNumberAsync(string caseNumber)
        {
            var result = await dbContext.Cases
                .Where(x => x.Number == caseNumber)
                .Select(x => new CaseViewModel
                {
                    ReportedAt = x.ReportedAt,
                    Id = x.Id,
                    LastUpdatedUtc = x.LastUpdatedUtc,
                    Number = x.Number,
                    AssignedProcessor = x.AssignedProcessor,
                    ChangedByUser = x.User == null ? null : (x.User.FullName ?? null),
                    Notes = x.Notes,
                    Priority = x.Priority.CasePriorityName,
                    ResumeAtUtc = x.ResumeAt,
                    Status = x.Status.CaseStatusName,
                    Subject = x.Subject,
                    WaitingReason = x.WaitingReason == null ? null : (x.WaitingReason.WaitingReasonName ?? null),
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> GetCaseNumberByIdAsync(int caseId)
        {
            return await dbContext.Cases
                .Where(c => c.Id == caseId)
                .Select(c => c.Number)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCaseAsync(CaseTask task)
        {
            int result = new int();

            var caseRecordToUpdate = await dbContext.Cases
                .FirstOrDefaultAsync(c => c.Id == task.CaseId);

            if (caseRecordToUpdate != null)
            {
                caseRecordToUpdate.PriorityId = task.PriorityId;
                caseRecordToUpdate.Subject = task.Subject;
                caseRecordToUpdate.StatusId = task.StatusId;
                caseRecordToUpdate.WaitingReasonId = task.WaitingReasonId;
                caseRecordToUpdate.TypeId = task.TypeId;
                caseRecordToUpdate.Queue = task.Queue;
                caseRecordToUpdate.QueueStatusId = task.QueueStatusId;
                caseRecordToUpdate.LastUpdatedUtc = task.LastUpdatedUtc;
                caseRecordToUpdate.ResumeAt = task.ResumeAt;
                caseRecordToUpdate.AssignedProcessor = task.AssignedProcessor;
                caseRecordToUpdate.UserId = task.UserId;
                caseRecordToUpdate.Notes = task.Notes;

                dbContext.Cases.Update(caseRecordToUpdate);
                result = await dbContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<int>> GetAllCaseStatusesIdsAsync()
        {
            return await dbContext.CaseStatuses
                .Select(cs => cs.Id)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<int>> GetAllCasePrioritiesIdsAsync()
        {
            return await dbContext.CasePriorities
                .Select(cp => cp.Id)
                .ToArrayAsync();
        }

        public async Task<CaseViewUpdateViewModel> GetCaseByIdAsync(int id)
        {
            var result = await dbContext.Cases
                .Where(x => x.Id == id)
                .Select(x => new CaseViewUpdateViewModel
                {
                    Id = x.Id,
                    AssignedProcessor = x.AssignedProcessor,
                    ChangedByUser = x.User == null ? null : (x.User.FullName ?? null),
                    Notes = x.Notes,
                    Number = x.Number,
                    Priority = x.Priority.CasePriorityName,
                    Queue = x.Queue,
                    QueueStatus = x.QueueStatus == null ? null : (x.QueueStatus.QueueStatusName ?? null),
                    ReportedAtUtc = x.ReportedAt,
                    ResumeAt = x.ResumeAt,
                    Status = x.Status.CaseStatusName,
                    Subject = x.Subject,
                    Type = x.Type.CaseTypeName,
                    UpdatedAtUtc = x.LastUpdatedUtc,
                    WaitingReason = x.WaitingReason == null ? null : (x.WaitingReason.WaitingReasonName ?? null),
                    Tasks = x.Tasks
                        .OrderByDescending(x => x.LastUpdatedUtc)
                        .Select(x => new TaskViewModel
                        {
                            AssignedProcessor = x.AssignedProcessor,
                            ChangedByUser = x.User == null ? null : (x.User.FullName ?? null),
                            Notes = x.Notes,
                            CaseNumber = x.Case.Number,
                            CasePriority = x.Priority.CasePriorityName,
                            CaseStatus = x.Status.CaseStatusName,
                            Subject = x.Subject,
                            WaitingReason = x.WaitingReason == null ? null : (x.WaitingReason.WaitingReasonName ?? null),
                            LastUpdatedUtc = x.LastUpdatedUtc,
                            ResumeAtUtc = x.ResumeAt,
                        })
                        .ToArray()
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<QueueStatusViewModel>> GetAllQueueStatusesAsync()
        {
            var result = await dbContext.QueueStatuses
                .Select(x => new QueueStatusViewModel
                {
                    Id = x.Id,
                    QueueStatusName = x.QueueStatusName,
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<WaitingReasonViewModel>> GetAllWaitingReasonsAsync()
        {
            var result = await dbContext.WaitingReasons
                .Select(x => new WaitingReasonViewModel
                {
                    Id = x.Id,
                    WaitingReasonName = x.WaitingReasonName,
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<DateTime> GetCaseReportedAtAsync(int caseId)
        {
            var result = await dbContext.Cases
                .Where(x => x.Id == caseId).Select(x => x.ReportedAt).FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> GetCaseNumberAsync(int caseId)
        {
            var result = await dbContext.Cases
                .Where(x => x.Id == caseId).Select(x => x.Number).FirstOrDefaultAsync();

            return result;
        }
    }
}
