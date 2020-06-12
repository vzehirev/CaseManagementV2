using CaseManagement.Models.TaskModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.CaseModels
{
    public class Case
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public int SNo { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int PriorityId { get; set; }

        public virtual CasePriority Priority { get; set; }

        public string Subject { get; set; }

        public int StatusId { get; set; }

        public virtual CaseStatus Status { get; set; }

        public int? WaitingReasonId { get; set; }

        public virtual WaitingReason WaitingReason { get; set; }

        public DateTime ReportedAt { get; set; }

        public DateTime? LastUpdatedUtc { get; set; }

        public DateTime? ResumeAt { get; set; }

        public int TypeId { get; set; }

        public virtual CaseType Type { get; set; }

        public string Queue { get; set; }

        public int? QueueStatusId { get; set; }

        public virtual QueueStatus QueueStatus { get; set; }

        public string AssignedProcessor { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<CaseTask> Tasks { get; set; } = new HashSet<CaseTask>();
    }
}
