using CaseManagement.ViewModels.SpcAgentAssignment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Services.SpcAgentAssignment
{
    public interface ISpcAgentAssignmentService
    {
        Task<IEnumerable<SpcAgentAvailabiltyAndSkillsViewModel>> GetAllAgentsAvailabilityAndSkillsAsync();

        Task<int> UpdateAgentsAvailabilityAndSkillsAsync(
            IEnumerable<SpcAgentAvailabiltyAndSkillsViewModel> agentsAvailabiltyAndSkills,
            string changedByUser);
    }
}
