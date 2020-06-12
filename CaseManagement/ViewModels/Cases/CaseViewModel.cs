using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.ViewModels.Cases
{
    public class CaseViewModel
    {
        public int Id { get; set; }

        public DateTime ReportedAt { get; set; }

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

        public string Number { get; set; }

        public string Subject { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string WaitingReason { get; set; }

        public string AssignedProcessor { get; set; }

        public string ChangedByUser { get; set; }

        public DateTime? ResumeAtUtc { get; set; }

        public string Notes { get; set; }
    }
}
