using CaseManagement.Data;
using CaseManagement.Models.AgentAssignment;
using CaseManagement.ViewModels.AgentAssignment;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.AgentAssignment
{
    public class AgentAssignmentService : IAgentAssignmentService
    {
        private readonly ApplicationDbContext dbContext;

        public AgentAssignmentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AgentAvailabiltyAndSkillsViewModel>> GetAllAgentsAvailabilityAndSkillsAsync()
        {
            return await this.dbContext.Users.Select(x => new AgentAvailabiltyAndSkillsViewModel
            {
                CUser = x.CUser,
                FullName = x.FullName,
                IsAvailable = x.AgentAvailabilityAndSkills.IsAvailable,
                TTES12 = x.AgentAvailabilityAndSkills.TTES12,
                TTES34 = x.AgentAvailabilityAndSkills.TTES34,
                RTPU12 = x.AgentAvailabilityAndSkills.RTPU12,
                RTPU34 = x.AgentAvailabilityAndSkills.RTPU34,
                EX12 = x.AgentAvailabilityAndSkills.EX12,
                EX34 = x.AgentAvailabilityAndSkills.EX34,
                Other12 = x.AgentAvailabilityAndSkills.Other12,
                Other34 = x.AgentAvailabilityAndSkills.Other34
            })
                .ToArrayAsync();
        }

        public async Task<int> UpdateAgentsAvailabilityAndSkillsAsync(IEnumerable<AgentAvailabiltyAndSkillsViewModel> agentsAvailabiltyAndSkills)
        {
            var agentsAvailabiltyAndSkillsDict = agentsAvailabiltyAndSkills
                .Where(x => x.CUser != null)
                .ToDictionary(x => x.CUser);

            var users = await this.dbContext.Users
                .Where(x => agentsAvailabiltyAndSkillsDict.Keys.Contains(x.CUser))
                .Include(x => x.AgentAvailabilityAndSkills)
                .ToArrayAsync();

            foreach (var user in users)
            {
                if (user.AgentAvailabilityAndSkills == null)
                {
                    user.AgentAvailabilityAndSkills = new AgentAvailabilityAndSkills();
                }

                user.AgentAvailabilityAndSkills.IsAvailable = agentsAvailabiltyAndSkillsDict[user.CUser].IsAvailable;
                user.AgentAvailabilityAndSkills.TTES12 = agentsAvailabiltyAndSkillsDict[user.CUser].TTES12;
                user.AgentAvailabilityAndSkills.TTES34 = agentsAvailabiltyAndSkillsDict[user.CUser].TTES34;
                user.AgentAvailabilityAndSkills.RTPU12 = agentsAvailabiltyAndSkillsDict[user.CUser].RTPU12;
                user.AgentAvailabilityAndSkills.RTPU34 = agentsAvailabiltyAndSkillsDict[user.CUser].RTPU34;
                user.AgentAvailabilityAndSkills.EX12 = agentsAvailabiltyAndSkillsDict[user.CUser].EX12;
                user.AgentAvailabilityAndSkills.EX34 = agentsAvailabiltyAndSkillsDict[user.CUser].EX34;
                user.AgentAvailabilityAndSkills.Other12 = agentsAvailabiltyAndSkillsDict[user.CUser].Other12;
                user.AgentAvailabilityAndSkills.Other34 = agentsAvailabiltyAndSkillsDict[user.CUser].Other34;
            }

            this.dbContext.Users.UpdateRange(users);

            return await this.dbContext.SaveChangesAsync();
        }
    }
}
