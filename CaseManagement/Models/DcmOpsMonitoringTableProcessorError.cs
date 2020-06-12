using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Models
{
    public class DcmOpsMonitoringTableProcessorError
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; }

        [Required]
        public string ErrorMessage { get; set; }
    }
}
