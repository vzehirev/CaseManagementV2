using CaseManagement.Models;
using CaseManagement.Models.CaseModels;
using CaseManagement.Models.Datacenters;
using CaseManagement.Models.DateTimeConverter;
using CaseManagement.Models.DCMOpsMonitoringTable;
using CaseManagement.Models.TaskModels;
using CaseManagement.Models.TimeZoneRegions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CaseManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }

        public DbSet<Case> Cases { get; set; }

        public DbSet<CaseType> CaseTypes { get; set; }

        public DbSet<CasePhase> CasePhases { get; set; }

        public DbSet<CaseStatus> CaseStatuses { get; set; }

        public DbSet<CasePriority> CasePriorities { get; set; }

        public DbSet<ServiceArea> ServiceAreas { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<CaseTask> Tasks { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public DbSet<FieldModification> FieldModifications { get; set; }

        public DbSet<CaseModificationLogRecord> CaseModificationLogRecords { get; set; }

        public DbSet<TaskModificationLogRecord> TaskModificationLogRecords { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<TimeZoneRegion> TimeZoneRegions { get; set; }

        public DbSet<Datacenter> Datacenters { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<DCMOpsMonitoringRow> DCMOpsMonitoring { get; set; }

        public DbSet<QueueStatus> QueueStatuses { get; set; }

        public DbSet<WaitingReason> WaitingReasons { get; set; }

        public DbSet<DcmOpsMonitoringTableProcessorError> DcmOpsMonitoringTableProcessorErrors { get; set; }
    }
}
