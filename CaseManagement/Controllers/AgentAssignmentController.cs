using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.Models;
using CaseManagement.Services.AgentAssignment;
using CaseManagement.ViewModels.AgentAssignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.Controllers
{
    [Authorize]
    public class AgentAssignmentController : Controller
    {
        private readonly IAgentAssignmentService agentAssignmentService;
        private readonly UserManager<ApplicationUser> userManager;

        public AgentAssignmentController(IAgentAssignmentService agentAssignmentService, UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> UpdateAvailabilityAndSkills(IEnumerable<AgentAvailabiltyAndSkillsViewModel> agentsAvailabilityAndSkills)
        {
            await this.agentAssignmentService.UpdateAgentsAvailabilityAndSkillsAsync(agentsAvailabilityAndSkills, this.userManager.GetUserId(this.User));
            
            this.TempData["UpdateSuccessful"] = true;

            return RedirectToAction(nameof(this.Index));
        }
    }
}
