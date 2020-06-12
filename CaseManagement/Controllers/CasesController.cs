using CaseManagement.Models;
using CaseManagement.Services;
using CaseManagement.Services.Announcements;
using CaseManagement.Services.CasePriorities;
using CaseManagement.Services.Cases;
using CaseManagement.Services.CaseStatuses;
using CaseManagement.Services.Users;
using CaseManagement.ViewModels.Cases;
using CaseManagement.ViewModels.Cases.Index;
using CaseManagement.ViewModels.Cases.Input;
using CaseManagement.ViewModels.Cases.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Controllers
{
    [Authorize]
    public class CasesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ICasesService casesService;
        private readonly ICaseStatusesService caseStatusesService;
        private readonly ICasePrioritiesService casePrioritiesService;
        private readonly IAnnouncementsService announcementsService;

        public CasesController(IUsersService usersService,
            ICasesService casesService,
            ICaseStatusesService caseStatusesService,
            ICasePrioritiesService casePrioritiesService,
            IAnnouncementsService announcementsService)
        {
            this.usersService = usersService;
            this.casesService = casesService;
            this.caseStatusesService = caseStatusesService;
            this.casePrioritiesService = casePrioritiesService;
            this.announcementsService = announcementsService;
        }

        public async Task<IActionResult> Index(CasesIndexViewModel viewModel)
        {
            const int casesPerPage = 10;

            string[] possibleOrders = new[]
            {
                "ReportedAt-desc",
                "ReportedAt-asc",
                "Status-desc",
                "Status-asc",
                "Priority-desc",
                "Priority-asc",
            };

            if (viewModel.PageNumber < 1)
            {
                viewModel.PageNumber = 1;
            }

            int skip = (viewModel.PageNumber - 1) * casesPerPage;

            if (!possibleOrders.Contains(viewModel.OrderedBy))
            {
                viewModel.OrderedBy = possibleOrders.First();
            }

            if (viewModel.SelectedStatuses == null)
            {
                viewModel.SelectedStatuses = await casesService.GetAllCaseStatusesIdsAsync();
            }

            if (viewModel.SelectedPriorities == null)
            {
                viewModel.SelectedPriorities = await casesService.GetAllCasePrioritiesIdsAsync();
            }

            viewModel.CasesCount = await casesService.GetCasesCountAsync(viewModel.SelectedStatuses, viewModel.SelectedPriorities);
            viewModel.Cases = await casesService.GetCasesAsync(skip, casesPerPage, viewModel.SelectedStatuses,
                viewModel.SelectedPriorities, viewModel.OrderedBy);
            viewModel.AllCaseStatuses = await caseStatusesService.GetAllCaseStatusesAsync();
            viewModel.AllCasePriorities = await casePrioritiesService.GetAllCasePrioritiesAsync();
            viewModel.LastPage = (int)Math.Ceiling((double)viewModel.CasesCount / casesPerPage);

            if (viewModel.LastPage == 0)
            {
                return View(viewModel);
            }

            viewModel.Announcements = await announcementsService.GetAnnouncementsAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            CreateCaseInputModel model = new CreateCaseInputModel
            {
                // Populate drop-down menus' options
                CaseTypes = await casesService.GetAllCaseTypesAsync(),
                CaseStatuses = await casesService.GetAllCaseStatusesAsync(),
                CasePriorities = await casesService.GetAllCasePrioritiesAsync(),
                CaseServices = await casesService.GetAllCaseServicesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCaseInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                // Populate drop-down menus' options and return create page to edit data and re-submit
                inputModel.CaseTypes = await casesService.GetAllCaseTypesAsync();
                inputModel.CaseStatuses = await casesService.GetAllCaseStatusesAsync();
                inputModel.CasePriorities = await casesService.GetAllCasePrioritiesAsync();
                inputModel.CaseServices = await casesService.GetAllCaseServicesAsync();

                return View(inputModel);
            }

            string userId = usersService.UserManager.GetUserId(User);

            int createResult = await casesService.CreateCaseAsync(inputModel, userId);

            if (createResult > 0)
            {
                await usersService.UpdateUserLastActivityDateAsync(userId);

                TempData["CaseCreatedSuccesffully"] = true;

                return RedirectToAction("ViewUpdate", new { id = createResult });
            }

            return View("Error", new ErrorViewModel());
        }

        public async Task<IActionResult> ViewUpdate(int id)
        {
            var viewModel = await casesService.GetCaseByIdAsync(id);

            return View(viewModel);
        }

        public async Task<IActionResult> SearchCase(string caseNumber)
        {

            if (string.IsNullOrWhiteSpace(caseNumber))
            {
                return RedirectToAction("Index");
            }

            var viewModel = await casesService.GetCaseByNumberAsync(caseNumber.Trim());

            return View("SearchCaseResults", viewModel);
        }
    }
}