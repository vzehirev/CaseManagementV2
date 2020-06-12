using CaseManagement.Models;
using CaseManagement.Services.Cases;
using CaseManagement.Services.Tasks;
using CaseManagement.Services.Users;
using CaseManagement.ViewModels.Tasks;
using CaseManagement.ViewModels.Tasks.Create;
using CaseManagement.ViewModels.Tasks.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ITasksService tasksService;
        private readonly ICasesService casesService;

        public TasksController(IUsersService usersService, ITasksService tasksService, ICasesService casesService)
        {
            this.usersService = usersService;
            this.tasksService = tasksService;
            this.casesService = casesService;
        }

        public async Task<IActionResult> Create(int caseId)
        {
            var viewModel = await tasksService.GenerateTaskCreateViewModel(caseId);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AllPriorities = await this.casesService.GetAllCasePrioritiesAsync();
                viewModel.AllQueueStatuses = await this.casesService.GetAllQueueStatusesAsync();
                viewModel.AllStatuses = await this.casesService.GetAllCaseStatusesAsync();
                viewModel.AllTypes = await this.casesService.GetAllCaseTypesAsync();
                viewModel.AllWaitingReasons = await this.casesService.GetAllWaitingReasonsAsync();

                return View(viewModel);
            }

            string userId = usersService.UserManager.GetUserId(User);
            int createResult = await tasksService.CreateTaskAsync(viewModel, userId);

            if (createResult > 0)
            {
                await usersService.UpdateUserLastActivityDateAsync(userId);

                TempData["TaskCreatedSuccessfully"] = true;

                return LocalRedirect($"/Cases/ViewUpdate/{viewModel.CaseId}#tasks-table");
            }

            return View("Error", new ErrorViewModel());
        }

        public async Task<IActionResult> ViewUpdate(int id)
        {
            ViewUpdateTaskIOModel outputModel = await tasksService.GetTaskByIdAsync(id);

            return View(outputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ViewUpdateTaskIOModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", new ErrorViewModel());
            }

            string userId = usersService.UserManager.GetUserId(User);
            int updateResult = await tasksService.UpdateTaskAsync(inputModel, userId);

            if (updateResult > 0)
            {
                await usersService.UpdateUserLastActivityDateAsync(userId);

                TempData["TaskUpdatedSuccessfully"] = true;
            }

            return LocalRedirect($"/Cases/ViewUpdate/{inputModel.CaseId}#tasks-table");
        }
    }
}