using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.Services.AgentAssignment;
using CaseManagement.ViewModels.AgentAssignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.Controllers
{
    [Authorize]
    public class AgentAssignmentController : Controller
    {
        private readonly IAgentAssignmentService agentAssignmentService;

        public AgentAssignmentController(IAgentAssignmentService agentAssignmentService)
        {
            this.agentAssignmentService = agentAssignmentService;
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
            await this.agentAssignmentService.UpdateAgentsAvailabilityAndSkillsAsync(agentsAvailabilityAndSkills);

            this.TempData["UpdateSuccessful"] = true;

            return RedirectToAction(nameof(this.Index));
        }
    }
}
