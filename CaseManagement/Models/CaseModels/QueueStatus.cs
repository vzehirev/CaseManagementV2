using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Models.CaseModels
{
    public class QueueStatus
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string QueueStatusName { get; set; }
    }
}
