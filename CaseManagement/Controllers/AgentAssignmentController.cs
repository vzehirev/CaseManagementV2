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

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                TicketTypes = await this.agentAssignmentService.GetAllTicketTypesAsync()
            };

            return View(viewModel);
        }
    }
}
