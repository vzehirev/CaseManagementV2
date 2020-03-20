﻿using CaseManagement.Data;
using CaseManagement.Models;
using CaseManagement.Models.CaseModels;
using CaseManagement.ViewModels.Cases;
using CaseManagement.ViewModels.Cases.Input;
using CaseManagement.ViewModels.Cases.Output;
using CaseManagement.ViewModels.Tasks.Output;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var caseToAdd = new Case
            {
                Number = inputModel.Number,
                Subject = inputModel.Subject,
                Description = inputModel.Description,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                TypeId = inputModel.TypeId,
                StatusId = inputModel.StatusId,
                PriorityId = inputModel.PriorityId,
                ServiceId = inputModel.ServiceId,
            };

            this.dbContext.Cases.Add(caseToAdd);
            var saveResult = await this.dbContext.SaveChangesAsync();

            if (saveResult > 0)
            {
                return caseToAdd.Id;
            }

            return saveResult;
        }

        public async Task<ICollection<CasePriority>> GetAllCasePrioritiesAsync()
        {
            return await this.dbContext.CasePriorities.ToArrayAsync();
        }

        public async Task<AllCasesOutputModel> GetAllCasesAsync()
        {
            var result = new AllCasesOutputModel
            {
                Cases = await this.dbContext.Cases
                .Select(c => new CaseOutputModel
                {
                    Id = c.Id,
                    Number = c.Number,
                    CreatedOn = c.CreatedOn,
                    Status = c.Status.Status,
                    Priority = c.Priority.Priority,
                    Subject = c.Subject,
                    Agent = c.User.Email,
                })
                .ToArrayAsync(),
            };

            return result;
        }

        public async Task<ICollection<Service>> GetAllCaseServicesAsync()
        {
            return await this.dbContext.Services.ToArrayAsync();
        }

        public async Task<ICollection<CaseStatus>> GetAllCaseStatusesAsync()
        {
            return await this.dbContext.CaseStatuses.ToArrayAsync();
        }

        public async Task<ICollection<CaseType>> GetAllCaseTypesAsync()
        {
            return await this.dbContext.CaseTypes.ToArrayAsync();
        }

        public async Task<ViewUpdateCaseIOModel> GetCaseByIdAsync(int id)
        {
            var outputModel = await this.dbContext.Cases
                .Select(c => new ViewUpdateCaseIOModel
                {
                    Id = c.Id,
                    Number = c.Number,
                    CreatedOn = c.CreatedOn,
                    TypeId = c.Type.Id,
                    StatusId = c.Status.Id,
                    PriorityId = c.Priority.Id,
                    PhaseId = c.Phase.Id,
                    ServiceId = c.Service.Id,
                    Subject = c.Subject,
                    Description = c.Description,
                    Tasks = c.Tasks.Select(t => new TaskOutputModel
                    {
                        Id = t.Id,
                        CreatedOn = t.CreatedOn,
                        Action = t.Action,
                        NextAction = t.NextAction,
                        Type = t.Type.Type,
                        Status = t.Status.Status,
                        Agent = t.User.Email
                    }).ToArray(),
                    CaseStatuses = this.dbContext.CaseStatuses.ToArray(),
                    CasePriorities = this.dbContext.CasePriorities.ToArray(),
                    CaseTypes = this.dbContext.CaseTypes.ToArray(),
                    CaseServices = this.dbContext.Services.ToArray(),
                })
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            return outputModel;
        }

        public async Task<SearchCaseResultsOutputModel> GetCasesByNumberAsync(string caseNumber)
        {
            var result = new SearchCaseResultsOutputModel
            {
                SearchedCaseNumber = caseNumber,
                Results = await this.dbContext.Cases
                .Where(c => c.Number == caseNumber)
                .Select(c => new CaseOutputModel
                {
                    Number = c.Number,
                    Priority = c.Priority.Priority,
                    Status = c.Status.Status,
                    CreatedOn = c.CreatedOn,
                    Id = c.Id,
                    Agent = c.User.Email,
                    Subject = c.Subject
                })
                .ToArrayAsync()
            };

            return result;
        }

        public async Task<string> GetCaseNumberByIdAsync(int caseId)
        {
            return await this.dbContext.Cases
                .Where(c => c.Id == caseId)
                .Select(c => c.Number)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCaseAsync(ViewUpdateCaseIOModel inputModel, string userId)
        {
            var caseRecordToUpdate = await this.dbContext.Cases
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);

            List<FieldModification> fieldModifications = new List<FieldModification>();

            if (caseRecordToUpdate.Number != inputModel.Number)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Number",
                    OldValue = caseRecordToUpdate.Number,
                    NewValue = inputModel.Number,
                });
            }

            if (caseRecordToUpdate.Description != inputModel.Description)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Description",
                    OldValue = caseRecordToUpdate.Description,
                    NewValue = inputModel.Description,
                });
            }

            if (caseRecordToUpdate.Subject != inputModel.Subject)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Subject",
                    OldValue = caseRecordToUpdate.Subject,
                    NewValue = inputModel.Subject,
                });
            }

            if (caseRecordToUpdate.StatusId != inputModel.StatusId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Status",
                    OldValue = await this.dbContext.CaseStatuses.Where(cs => cs.Id == caseRecordToUpdate.StatusId).Select(cs => cs.Status).FirstOrDefaultAsync(),
                    NewValue = await this.dbContext.CaseStatuses.Where(cs => cs.Id == inputModel.StatusId).Select(cs => cs.Status).FirstOrDefaultAsync(),
                });
            }

            if (caseRecordToUpdate.TypeId != inputModel.TypeId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Type",
                    OldValue = await this.dbContext.CaseTypes.Where(ct => ct.Id == caseRecordToUpdate.TypeId).Select(ct => ct.Type).FirstOrDefaultAsync(),
                    NewValue = await this.dbContext.CaseTypes.Where(ct => ct.Id == inputModel.TypeId).Select(ct => ct.Type).FirstOrDefaultAsync(),
                });
            }

            if (caseRecordToUpdate.PriorityId != inputModel.PriorityId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Priority",
                    OldValue = await this.dbContext.CasePriorities.Where(cp => cp.Id == caseRecordToUpdate.PriorityId).Select(cp => cp.Priority).FirstOrDefaultAsync(),
                    NewValue = await this.dbContext.CasePriorities.Where(cp => cp.Id == inputModel.PriorityId).Select(cp => cp.Priority).FirstOrDefaultAsync(),
                });
            }

            if (caseRecordToUpdate.PhaseId != inputModel.PhaseId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Phase",
                    OldValue = await this.dbContext.CasePhases.Where(cp => cp.Id == caseRecordToUpdate.PhaseId).Select(cp => cp.Phase).FirstOrDefaultAsync(),
                    NewValue = await this.dbContext.CasePhases.Where(cp => cp.Id == inputModel.PhaseId).Select(cp => cp.Phase).FirstOrDefaultAsync(),
                });
            }

            if (caseRecordToUpdate.ServiceId != inputModel.ServiceId)
            {
                fieldModifications.Add(new FieldModification
                {
                    FieldName = "Service",
                    OldValue = await this.dbContext.Services.Where(s => s.Id == caseRecordToUpdate.ServiceId).Select(s => s.ServiceName).FirstOrDefaultAsync(),
                    NewValue = await this.dbContext.Services.Where(s => s.Id == inputModel.ServiceId).Select(s => s.ServiceName).FirstOrDefaultAsync(),
                });
            }

            caseRecordToUpdate.Number = inputModel.Number;
            caseRecordToUpdate.Description = inputModel.Description;
            caseRecordToUpdate.Subject = inputModel.Subject;
            caseRecordToUpdate.LastModified = DateTime.UtcNow;
            caseRecordToUpdate.StatusId = inputModel.StatusId;
            caseRecordToUpdate.TypeId = inputModel.TypeId;
            caseRecordToUpdate.PriorityId = inputModel.PriorityId;
            caseRecordToUpdate.PhaseId = inputModel.PhaseId;
            caseRecordToUpdate.ServiceId = inputModel.ServiceId;

            if (fieldModifications.Count > 0)
            {
                var modificationLogRecord = new CaseModificationLogRecord
                {
                    ModificationTime = DateTime.UtcNow,
                    UserId = userId,
                    CaseId = caseRecordToUpdate.Id,
                    ModifiedFields = fieldModifications,
                };

                this.dbContext.CaseModificationLogRecords.Add(modificationLogRecord);
            }

            this.dbContext.Cases.Update(caseRecordToUpdate);
            var saveResult = await this.dbContext.SaveChangesAsync();

            return saveResult;
        }
    }
}
