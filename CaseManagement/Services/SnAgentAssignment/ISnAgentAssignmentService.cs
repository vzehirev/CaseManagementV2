using CaseManagement.ViewModels.SnAgentAssignment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Services.SnAgentAssignment
{
    public interface ISnAgentAssignmentService
    {
        Task<IEnumerable<SnAgentAvailabiltyAndSkillsViewModel>> GetAllAgentsAvailabilityAndSkillsAsync();

        Task<int> UpdateAgentsAvailabilityAndSkillsAsync(
            IEnumerable<SnAgentAvailabiltyAndSkillsViewModel> agentsAvailabiltyAndSkills,
            string changedByUser);
    }
}
