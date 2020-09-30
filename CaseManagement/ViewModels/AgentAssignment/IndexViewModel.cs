using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.ViewModels.AgentAssignment
{
    public class IndexViewModel
    {
        public IEnumerable<TicketTypeViewModel> TicketTypes { get; set; }
    }
}
