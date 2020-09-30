using CaseManagement.ViewModels.AgentAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.AgentAssignment
{
    public interface IAgentAssignmentService
    {
        Task<IEnumerable<TicketTypeViewModel>> GetAllTicketTypesAsync();
    }
}
