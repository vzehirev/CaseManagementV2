using CaseManagement.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CaseManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string userEmail)
        {
            await usersService.ResetUserPasswordByEmailAsync(userEmail.Trim());

            TempData["PasswordResetSuccessful"] = true;

            return LocalRedirect("/");
        }

        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> ResetAgentPassword(string userId)
        {
            await usersService.ResetUserPasswordByIdAsync(userId);

            TempData["AgentPasswordResetSuccessful"] = true;

            return LocalRedirect("/StatisticsAndReports/RegisteredAgents");
        }

        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> MakeLead(string userId)
        {
            await usersService.MakeLead(userId);

            TempData["MakeLeadSuccess"] = true;

            return LocalRedirect("/StatisticsAndReports/RegisteredAgents");
        }

        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> DeleteAgent(string userId)
        {
            await usersService.DeleteAgent(userId);

            TempData["DeleteAgentSuccess"] = true;

            return LocalRedirect("/StatisticsAndReports/RegisteredAgents");
        }

        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> RemoveLead(string userId)
        {
            await usersService.RemoveLead(userId);

            TempData["RemoveLeadSuccess"] = true;

            return LocalRedirect("/StatisticsAndReports/RegisteredAgents");
        }
    }
}