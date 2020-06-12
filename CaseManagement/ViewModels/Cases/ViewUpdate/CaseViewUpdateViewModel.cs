using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.ViewModels.Cases.ViewUpdate
{
    public class CaseViewUpdateViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Priority { get; set; }

        public string Subject { get; set; }

        public string Status { get; set; }

        public string WaitingReason { get; set; }

        public DateTime ReportedAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }

        public DateTime? UpdatedAtIst
        {
            get
            {
                if (UpdatedAtUtc != null)
                {
                    var result = UpdatedAtUtc.Value
                        .AddHours(GlobalConstants.IstHoursOffsetFromUtc)
                        .AddMinutes(GlobalConstants.IstMinutesOffsetFromUtc);

                    return result;
                }

                return null;
            }
        }

        public DateTime? ResumeAt { get; set; }

        public string Type { get; set; }

        public string Queue { get; set; }

        public string QueueStatus { get; set; }

        public string AssignedProcessor { get; set; }

        public string ChangedByUser { get; set; }

        public string Notes { get; set; }

        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}
