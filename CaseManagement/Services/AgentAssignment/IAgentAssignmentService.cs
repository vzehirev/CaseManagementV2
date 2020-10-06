using CaseManagement.ViewModels.AgentAssignment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Services.AgentAssignment
{
    public interface IAgentAssignmentService
    {
        Task<IEnumerable<AgentAvailabiltyAndSkillsViewModel>> GetAllAgentsAvailabilityAndSkillsAsync();

        Task<int> UpdateAgentsAvailabilityAndSkillsAsync(IEnumerable<AgentAvailabiltyAndSkillsViewModel> agentsAvailabiltyAndSkills);
    }
}
