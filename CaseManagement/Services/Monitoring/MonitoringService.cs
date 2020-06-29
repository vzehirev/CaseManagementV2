using CaseManagement.Data;
using CaseManagement.ViewModels.Monitoring;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.Monitoring
{
    public class MonitoringService : IMonitoringService
    {
        private readonly ApplicationDbContext dbContext;

        public MonitoringService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<InProcessTicketViewModel>> GetInProcessTicketsAsync()
        {
            var inProcessTickets = await this.dbContext.DCMOpsMonitoring
                .FromSqlRaw("SELECT * FROM DCMOpsMonitoring WHERE SNo IN" +
                    "(SELECT MAX(SNo) FROM DCMOpsMonitoring WHERE Queue = 'DATA CENTER MANAGEMENT OPERATIONS CENTER' AND (Status = 'In Process' OR Status = 'New') GROUP BY TicketID)" +
                    "ORDER BY UploadTimeIST, Priority")
                .ToArrayAsync();

            var result = inProcessTickets.Select(x => new InProcessTicketViewModel
            {
                UploadTimeIST = x.UploadTimeIST == null ? "" : ((DateTime)x.UploadTimeIST).ToString("dd-MMM-yyyy HH:mm"),
                TicketId = x.TicketID,
                ReportedDate = x.ReportedDate == null ? "" : ((DateTime)x.ReportedDate).ToString("dd-MMM-yyyy HH:mm"),
                Priority = x.Priority.Trim(),
                Status = x.Status.Trim(),
                WaitingReason = x.WaitingReason.Trim(),
                Subject = x.Subject.Trim(),
                Assigned = x.Assigned.Trim(),
                ChangedBy = x.Changedby.Trim(),
                ResumeAt = x.Resumeat == null ? "" : ((DateTime)x.Resumeat).ToString("dd-MMM-yyyy HH:mm"),
                Notes = x.Notes.Trim(),
            })
                .ToArray();

            return result;
        }

        public async Task<IEnumerable<WaitingTicketViewModel>> GetWaitingTicketsAsync()
        {
            var waitingTickets = await this.dbContext.DCMOpsMonitoring
                .FromSqlRaw("SELECT * FROM DCMOpsMonitoring WHERE SNo IN (SELECT MAX(SNo) FROM DCMOpsMonitoring WHERE Queue = 'DATA CENTER MANAGEMENT OPERATIONS CENTER' AND (Status = 'Waiting') GROUP BY TicketID)" +
                    "ORDER BY Priority")
                .ToArrayAsync();

            var result = waitingTickets.Select(x => new WaitingTicketViewModel
            {
                UploadTimeIST = x.UploadTimeIST == null ? "" : ((DateTime)x.UploadTimeIST).ToString("dd-MMM-yyyy HH:mm"),
                HoldHours = x.HoldHours,
                TicketId = x.TicketID,
                ReportedDate = x.ReportedDate == null ? "" : ((DateTime)x.ReportedDate).ToString("dd-MMM-yyyy HH:mm"),
                Priority = x.Priority.Trim(),
                Status = x.Status.Trim(),
                WaitingReason = x.WaitingReason.Trim(),
                Subject = x.Subject.Trim(),
                Assigned = x.Assigned.Trim(),
                ChangedBy = x.Changedby.Trim(),
                ResumeAt = x.Resumeat == null ? "" : ((DateTime)x.Resumeat).ToString("dd-MMM-yyyy HH:mm"),
                Notes = x.Notes.Trim(),
            })
                .ToArray();

            return result;
        }
    }
}
