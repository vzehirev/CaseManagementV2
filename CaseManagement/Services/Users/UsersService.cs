using CaseManagement.Data;
using CaseManagement.Models;
using CaseManagement.ViewModels.Agents;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;

        public UserManager<ApplicationUser> UserManager { get; }

        public UsersService(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.UserManager = userManager;
        }

        public async Task<bool> ResetUserPasswordByIdAsync(string userId)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(userId);

            var removePassResult = await this.UserManager.RemovePasswordAsync(user);
            var setPassResult = await this.UserManager.AddPasswordAsync(user, configuration["DefaultResetPassword"]);

            return removePassResult.Succeeded && setPassResult.Succeeded;
        }

        public async Task<bool> ResetUserPasswordByEmailAsync(string userEmail)
        {
            ApplicationUser user = await this.UserManager.FindByEmailAsync(userEmail.Trim());

            var result = await ResetUserPasswordByIdAsync(user.Id);

            return result;
        }

        public async Task<bool> UpdateUserLastActivityDateAsync(string userId)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(userId);
            user.LastActivity = DateTime.UtcNow;
            var result = await this.UserManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<IEnumerable<ReportsRegisteredAgentsAgentViewModel>> GetAllAgentsAsync()
        {
            return await this.dbContext.Users
                .Where(x=>!x.IsDeleted)
                .Select(x => new ReportsRegisteredAgentsAgentViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                    CUser = x.CUser
                })
                .ToArrayAsync();
        }

        public async Task<IEnumerable<ReportsAgentsActivitiesAgentViewModel>> GetAllAgentsIdAndFullNameAsync()
        {
            return await this.dbContext.Users
                .Where(x => !x.IsDeleted)
                .Select(x => new ReportsAgentsActivitiesAgentViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                })
                .ToArrayAsync();
        }

        public async Task<bool> MakeLead(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            var result = await this.UserManager.AddToRoleAsync(user, "Lead");
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAgent(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            user.IsDeleted = true;

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveLead(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            var result = await this.UserManager.RemoveFromRoleAsync(user, "Lead");
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
