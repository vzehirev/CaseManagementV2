using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.CaseModels
{
    public class CaseType
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string CaseTypeName { get; set; }
    }
}