using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.CaseStatuses;
using CaseManagement.ViewModels.CaseType;
using CaseManagement.ViewModels.QueueStatus;
using CaseManagement.ViewModels.WaitingReason;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaseManagement.ViewModels.Tasks.Create
{
    public class TaskCreateViewModel
    {
        [Required]
        public int CaseId { get; set; }

        public string CaseNumber { get; set; }

        [Required]
        public DateTime ReportedAt { get; set; }

        public IEnumerable<CasePriorityViewModel> AllPriorities { get; set; }

        public IEnumerable<CaseStatusViewModel> AllStatuses { get; set; }

        public IEnumerable<QueueStatusViewModel> AllQueueStatuses { get; set; }

        public IEnumerable<WaitingReasonViewModel> AllWaitingReasons { get; set; }

        public IEnumerable<CaseTypeViewModel> AllTypes { get; set; }

        [Required]
        public int SelectedPriorityId { get; set; }

        [Required]
        public int SelectedStatusId { get; set; }

        public int? SelectedQueueStatusId { get; set; }

        public int? SelectedWaitingReasonId { get; set; }

        [Required]
        public int SelectedTypeId { get; set; }

        public string Subject { get; set; }

        public string Queue { get; set; }

        public DateTime? ResumeAt { get; set; }

        public string AssignedProcessor { get; set; }

        public string Notes { get; set; }
    }
}
