using CaseManagement.Models.TaskModels;
using CaseManagement.ViewModels.Tasks;
using CaseManagement.ViewModels.Tasks.Create;
using CaseManagement.ViewModels.Tasks.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Services.Tasks
{
    public interface ITasksService
    {
        public Task<int> CreateTaskAsync(TaskCreateViewModel viewModel, string userId);

        public Task<ViewUpdateTaskIOModel> GetTaskByIdAsync(int id);

        public Task<int> UpdateTaskAsync(ViewUpdateTaskIOModel taskToUpdate, string userId);

        public Task<ICollection<TaskType>> GetAllTaskTypesAsync();

        public Task<ICollection<Models.TaskModels.TaskStatus>> GetAllTaskStatusesAsync();

        public Task<TaskCreateViewModel> GenerateTaskCreateViewModel(int caseId);
    }
}
