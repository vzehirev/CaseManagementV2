using CaseManagement.Models.CaseModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.TaskModels
{
    public class CaseTask
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        public virtual Case Case { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int PriorityId { get; set; }

        public virtual CasePriority Priority { get; set; }

        public string Subject { get; set; }

        public int StatusId { get; set; }

        public virtual CaseStatus Status { get; set; }

        public int? WaitingReasonId { get; set; }

        public virtual WaitingReason WaitingReason { get; set; }

        [Required]
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
    }
}
