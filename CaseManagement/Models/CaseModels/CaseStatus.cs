using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.CaseModels
{
    public class CaseStatus
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string CaseStatusName { get; set; }
    }
}