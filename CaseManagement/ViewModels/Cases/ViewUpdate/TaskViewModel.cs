using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.ViewModels.Cases.ViewUpdate
{
    public class TaskViewModel
    {
        public DateTime? LastUpdatedUtc { get; set; }

        public DateTime? LastUpdatedIst
        {
            get
            {
                if (LastUpdatedUtc != null)
                {
                    var result = LastUpdatedUtc.Value
                        .AddHours(GlobalConstants.IstHoursOffsetFromUtc)
                        .AddMinutes(GlobalConstants.IstMinutesOffsetFromUtc);

                    return result;
                }

                return null;
            }
        }

        public string CaseNumber { get; set; }

        public string Subject { get; set; }

        public string CaseStatus { get; set; }

        public string CasePriority { get; set; }

        public string WaitingReason { get; set; }

        public string AssignedProcessor { get; set; }

        public string ChangedByUser { get; set; }

        public DateTime? ResumeAtUtc { get; set; }

        public string Notes { get; set; }
    }
}
