using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.Models;
using CaseManagement.Services.SpcAgentAssignment;
using CaseManagement.ViewModels.SpcAgentAssignment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.Controllers
{
    public class SpcAgentAssignmentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISpcAgentAssignmentService spcAgentAssignmentService;

        public SpcAgentAssignmentController(UserManager<ApplicationUser> userManager,
            ISpcAgentAssignmentService spcAgentAssignmentService)
        {
            this.userManager = userManager;
            this.spcAgentAssignmentService = spcAgentAssignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var agentsAvailabilityAndSkills = await this.spcAgentAssignmentService.GetAllAgentsAvailabilityAndSkillsAsync();
            var viewModel = new IndexViewModel
            {
                AgentsAvailabilityAndSkills = agentsAvailabilityAndSkills.ToArray()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvailabilityAndSkills(IEnumerable<SpcAgentAvailabiltyAndSkillsViewModel> agentsAvailabilityAndSkills)
        {
            await this.spcAgentAssignmentService.UpdateAgentsAvailabilityAndSkillsAsync(agentsAvailabilityAndSkills, this.userManager.GetUserId(this.User));

            this.TempData["UpdateSuccessful"] = true;

            return RedirectToAction(nameof(this.Index));
        }
    }
}
