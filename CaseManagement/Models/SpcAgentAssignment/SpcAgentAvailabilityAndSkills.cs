using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.SpcAgentAssignment
{
    public class SpcAgentAvailabilityAndSkills
    {

        [Required, Key]
        public int Id { get; set; }

        public bool IsAvailable { get; set; }

        public bool Urgent { get; set; }

        public bool Immediate { get; set; }

        public bool Normal { get; set; }

        public bool Low { get; set; }
    }
}
