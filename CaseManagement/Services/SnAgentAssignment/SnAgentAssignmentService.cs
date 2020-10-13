using CaseManagement.Data;
using CaseManagement.Models.SnAgentAssignment;
using CaseManagement.ViewModels.SnAgentAssignment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.SnAgentAssignment
{
    public class SnAgentAssignmentService : ISnAgentAssignmentService
    {
        private readonly ApplicationDbContext dbContext;

        public SnAgentAssignmentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SnAgentAvailabiltyAndSkillsViewModel>> GetAllAgentsAvailabilityAndSkillsAsync()
        {
            return await this.dbContext.Users.Select(x => new SnAgentAvailabiltyAndSkillsViewModel
            {
                UserId = x.Id,
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
            IEnumerable<SnAgentAvailabiltyAndSkillsViewModel> newAgentsAvailabiltyAndSkills,
            string changedByUser)
        {
            var newAgentsAvailabiltyAndSkillsDict = newAgentsAvailabiltyAndSkills.ToDictionary(x => x.UserId);

            var agentsAvailabiltyAndSkills = await this.dbContext.Users
                .Where(x => newAgentsAvailabiltyAndSkillsDict.Keys.Contains(x.Id))
                .Include(x => x.AgentAvailabilityAndSkills)
                .ToArrayAsync();

            var skills = typeof(SnAgentAvailabilityAndSkills).GetProperties()
                .Where(x => x.PropertyType == typeof(bool)).ToArray();

            foreach (var user in agentsAvailabiltyAndSkills)
            {
                if (user.AgentAvailabilityAndSkills == null)
                {
                    user.AgentAvailabilityAndSkills = new SnAgentAvailabilityAndSkills();
                }

                foreach (var skill in skills)
                {
                    var value = (bool)typeof(SnAgentAvailabilityAndSkills).GetProperty(skill.Name).GetValue(user.AgentAvailabilityAndSkills);
                    var newValue = (bool)typeof(SnAgentAvailabiltyAndSkillsViewModel).GetProperty(skill.Name).GetValue(newAgentsAvailabiltyAndSkillsDict[user.Id]);

                    if (value != newValue)
                    {
                        var changeLog = new SnAgentAvailabilityAndSkillsChangeLog
                        {
                            ChangedByUserId = changedByUser,
                            UserId = user.Id,
                            ChangedSkill = skill.Name,
                            OldValue = value,
                            NewValue = newValue
                        };

                        typeof(SnAgentAvailabilityAndSkills).GetProperty(skill.Name).SetValue(user.AgentAvailabilityAndSkills, newValue);

                        this.dbContext.AgentsAvailabilityAndSkillsChangeLogs.Add(changeLog);
                    }
                }
            }

            this.dbContext.Users.UpdateRange(agentsAvailabiltyAndSkills);

            return await this.dbContext.SaveChangesAsync();
        }
    }
}
