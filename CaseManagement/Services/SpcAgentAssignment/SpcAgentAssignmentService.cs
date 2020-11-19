using CaseManagement.Data;
using CaseManagement.Models.SpcAgentAssignment;
using CaseManagement.ViewModels.SpcAgentAssignment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.SpcAgentAssignment
{
    public class SpcAgentAssignmentService : ISpcAgentAssignmentService
    {
        private readonly ApplicationDbContext dbContext;

        public SpcAgentAssignmentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SpcAgentAvailabiltyAndSkillsViewModel>> GetAllAgentsAvailabilityAndSkillsAsync()
        {
            return await this.dbContext.Users.Where(x => !x.IsDeleted)
                .Select(x => new SpcAgentAvailabiltyAndSkillsViewModel
                {
                    UserId = x.Id,
                    CUser = x.CUser,
                    FullName = x.FullName,
                    IsAvailable = x.SpcAgentAvailabilityAndSkills.IsAvailable,
                    Urgent = x.SpcAgentAvailabilityAndSkills.Urgent,
                    Immediate = x.SpcAgentAvailabilityAndSkills.Immediate,
                    Normal = x.SpcAgentAvailabilityAndSkills.Normal,
                    Low = x.SpcAgentAvailabilityAndSkills.Low
                })
                .OrderByDescending(x => x.IsAvailable)
                .ToArrayAsync();
        }

        public async Task<int> UpdateAgentsAvailabilityAndSkillsAsync(
            IEnumerable<SpcAgentAvailabiltyAndSkillsViewModel> newAgentsAvailabiltyAndSkills,
            string changedByUser)
        {
            var newAgentsAvailabiltyAndSkillsDict = newAgentsAvailabiltyAndSkills.ToDictionary(x => x.UserId);

            var agentsAvailabiltyAndSkills = await this.dbContext.Users
                .Where(x => newAgentsAvailabiltyAndSkillsDict.Keys.Contains(x.Id))
                .Include(x => x.SpcAgentAvailabilityAndSkills)
                .ToArrayAsync();

            var skills = typeof(SpcAgentAvailabilityAndSkills).GetProperties()
                .Where(x => x.PropertyType == typeof(bool)).ToArray();

            foreach (var user in agentsAvailabiltyAndSkills)
            {
                if (user.SpcAgentAvailabilityAndSkills == null)
                {
                    user.SpcAgentAvailabilityAndSkills = new SpcAgentAvailabilityAndSkills();
                }

                foreach (var skill in skills)
                {
                    var value = (bool)typeof(SpcAgentAvailabilityAndSkills).GetProperty(skill.Name).GetValue(user.SpcAgentAvailabilityAndSkills);
                    var newValue = (bool)typeof(SpcAgentAvailabiltyAndSkillsViewModel).GetProperty(skill.Name).GetValue(newAgentsAvailabiltyAndSkillsDict[user.Id]);

                    if (value != newValue)
                    {
                        var changeLog = new SpcAgentAvailabilityAndSkillsChangeLog
                        {
                            ChangedByUserId = changedByUser,
                            UserId = user.Id,
                            ChangedSkill = skill.Name,
                            OldValue = value,
                            NewValue = newValue
                        };

                        typeof(SpcAgentAvailabilityAndSkills).GetProperty(skill.Name).SetValue(user.SpcAgentAvailabilityAndSkills, newValue);

                        this.dbContext.SpcAgentsAvailabilityAndSkillsChangeLogs.Add(changeLog);
                    }
                }
            }

            this.dbContext.Users.UpdateRange(agentsAvailabiltyAndSkills);

            return await this.dbContext.SaveChangesAsync();
        }
    }
}
