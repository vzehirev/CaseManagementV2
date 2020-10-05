using CaseManagement.Data;
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
    }
}
