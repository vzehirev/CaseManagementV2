using CaseManagement.Data;
using CaseManagement.Models.AgentAssignment;
using CaseManagement.ViewModels.AgentAssignment;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async Task<int> UpdateAgentsAvailabilityAndSkillsAsync(
            IEnumerable<AgentAvailabiltyAndSkillsViewModel> newAgentsAvailabiltyAndSkills,
            string changedByUser)
        {
            var newAgentsAvailabiltyAndSkillsDict = newAgentsAvailabiltyAndSkills
                .Where(x => x.CUser != null)
                .ToDictionary(x => x.CUser);

            var agentsAvailabiltyAndSkills = await this.dbContext.Users
                .Where(x => newAgentsAvailabiltyAndSkillsDict.Keys.Contains(x.CUser))
                .Include(x => x.AgentAvailabilityAndSkills)
                .ToArrayAsync();

            var skills = typeof(AgentAvailabilityAndSkills).GetProperties()
                .Where(x => x.PropertyType == typeof(bool)).ToArray();

            foreach (var user in agentsAvailabiltyAndSkills)
            {
                if (user.AgentAvailabilityAndSkills == null)
                {
                    user.AgentAvailabilityAndSkills = new AgentAvailabilityAndSkills();
                }

                foreach (var skill in skills)
                {
                    var value = (bool)typeof(AgentAvailabilityAndSkills).GetProperty(skill.Name).GetValue(user.AgentAvailabilityAndSkills);
                    var newValue = (bool)typeof(AgentAvailabiltyAndSkillsViewModel).GetProperty(skill.Name).GetValue(newAgentsAvailabiltyAndSkillsDict[user.CUser]);

                    if (value != newValue)
                    {
                        var changeLog = new AgentAvailabilityAndSkillsChangeLog
                        {
                            ChangedByUserId = changedByUser,
                            UserId = user.Id,
                            ChangedSkill = skill.Name,
                            OldValue = value,
                            NewValue = newValue
                        };

                        typeof(AgentAvailabilityAndSkills).GetProperty(skill.Name).SetValue(user.AgentAvailabilityAndSkills, newValue);

                        this.dbContext.AgentsAvailabilityAndSkillsChangeLogs.Add(changeLog);
                    }
                }
            }

            this.dbContext.Users.UpdateRange(agentsAvailabiltyAndSkills);

            return await this.dbContext.SaveChangesAsync();
        }
    }
}
