using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.Models;
using CaseManagement.Services.SnAgentAssignment;
using CaseManagement.ViewModels.SnAgentAssignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.Controllers
{
    [Authorize]
    public class SnAgentAssignmentController : Controller
    {
        private readonly ISnAgentAssignmentService agentAssignmentService;
        private readonly UserManager<ApplicationUser> userManager;

        public SnAgentAssignmentController(ISnAgentAssignmentService agentAssignmentService, UserManager<ApplicationUser> userManager)
        {
            this.agentAssignmentService = agentAssignmentService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var agentsAvailabilityAndSkills = await this.agentAssignmentService.GetAllAgentsAvailabilityAndSkillsAsync();
            var viewModel = new IndexViewModel
            {
                AgentsAvailabilityAndSkills = agentsAvailabilityAndSkills.ToArray()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvailabilityAndSkills(IEnumerable<SnAgentAvailabiltyAndSkillsViewModel> agentsAvailabilityAndSkills)
        {
            await this.agentAssignmentService.UpdateAgentsAvailabilityAndSkillsAsync(agentsAvailabilityAndSkills, this.userManager.GetUserId(this.User));
            
            this.TempData["UpdateSuccessful"] = true;

            return RedirectToAction(nameof(this.Index));
        }
    }
}
