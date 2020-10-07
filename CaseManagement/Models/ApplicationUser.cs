using CaseManagement.Models.AgentAssignment;
using CaseManagement.Models.CaseModels;
using CaseManagement.Models.TaskModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime LastActivity { get; set; }

        public string CUser { get; set; }

        public int? AgentAvailabilityAndSkillsId { get; set; }

        public AgentAvailabilityAndSkills AgentAvailabilityAndSkills { get; set; }

        public DateTime? LastTicketAssignedAt { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Join(' ', FirstName, LastName);
            }
        }

        public virtual ICollection<Case> Cases { get; set; } = new HashSet<Case>();

        public virtual ICollection<CaseTask> Tasks { get; set; } = new HashSet<CaseTask>();

        public virtual ICollection<CaseModificationLogRecord> ModifiedCases { get; set; } = new HashSet<CaseModificationLogRecord>();

        public virtual ICollection<TaskModificationLogRecord> ModifiedTasks { get; set; } = new HashSet<TaskModificationLogRecord>();
    }
}
