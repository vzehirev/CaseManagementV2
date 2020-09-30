using CaseManagement.Data;
using CaseManagement.ViewModels.AgentAssignment;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IEnumerable<TicketTypeViewModel>> GetAllTicketTypesAsync()
        {
            return await this.dbContext.TicketTypes
                .Select(x => new TicketTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToArrayAsync();
        }
    }
}
