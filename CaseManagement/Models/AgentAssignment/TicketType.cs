using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.AgentAssignment
{
    public class TicketType
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
