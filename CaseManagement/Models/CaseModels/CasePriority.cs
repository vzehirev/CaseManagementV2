using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.CaseModels
{
    public class CasePriority
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string CasePriorityName { get; set; }
    }
}