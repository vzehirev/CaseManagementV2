using CaseManagement.Models.CaseModels;
using CaseManagement.Models.TaskModels;
using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.Cases;
using CaseManagement.ViewModels.Cases.Index;
using CaseManagement.ViewModels.Cases.Input;
using CaseManagement.ViewModels.Cases.Output;
using CaseManagement.ViewModels.Cases.ViewUpdate;
using CaseManagement.ViewModels.CaseStatuses;
using CaseManagement.ViewModels.CaseType;
using CaseManagement.ViewModels.QueueStatus;
using CaseManagement.ViewModels.WaitingReason;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Services.Cases
{
    public interface ICasesService
    {
        public Task<int> CreateCaseAsync(CreateCaseInputModel inputModel, string userId);

        public Task<AllCasesOutputModel> GetCasesAsync(int skip, int take, string orderBy,
            IEnumerable<int> selectedStatuses, IEnumerable<int> selectedPriorities, string userId);

        public Task<CaseViewModel> GetCaseByNumberAsync(string caseNumber);

        public Task<ViewUpdateCaseIOModel> GetCaseByIdAsync(int id, int skipTasks, int takeTasks);

        public Task<int> UpdateCaseAsync(CaseTask task);

        public Task<IEnumerable<CaseTypeViewModel>> GetAllCaseTypesAsync();

        public Task<IEnumerable<WaitingReasonViewModel>> GetAllWaitingReasonsAsync();

        public Task<IEnumerable<CaseStatusViewModel>> GetAllCaseStatusesAsync();

        public Task<IEnumerable<QueueStatusViewModel>> GetAllQueueStatusesAsync();

        Task<DateTime> GetCaseReportedAtAsync(int caseId);

        Task<string> GetCaseNumberAsync(int caseId);

        public Task<IEnumerable<CasePriorityViewModel>> GetAllCasePrioritiesAsync();

        public Task<IEnumerable<Service>> GetAllCaseServicesAsync();

        public Task<string> GetCaseNumberByIdAsync(int caseId);

        public Task<IEnumerable<int>> GetAllCaseStatusesIdsAsync();

        public Task<IEnumerable<int>> GetAllCasePrioritiesIdsAsync();

        public Task<CaseViewUpdateViewModel> GetCaseByIdAsync(int id);

        public Task<int> GetCasesCountAsync(IEnumerable<int> selectedStatuses, IEnumerable<int> selectedPriorities);

        public Task<IEnumerable<CaseViewModel>> GetCasesAsync(int skip, int take,
            IEnumerable<int> selectedStatuses, IEnumerable<int> selectedPriorities, string orderBy);
    }
}
