using CaseManagement.Crons.DcmOpsMonitoringTableDataAutoProcessor;
using CaseManagement.Data;
using CaseManagement.Models;
using CaseManagement.Services;
using CaseManagement.Services.AgentAssignment;
using CaseManagement.Services.Announcements;
using CaseManagement.Services.CasePriorities;
using CaseManagement.Services.Cases;
using CaseManagement.Services.CaseStatuses;
using CaseManagement.Services.DatacentersService;
using CaseManagement.Services.DateTimeConverter;
using CaseManagement.Services.Monitoring;
using CaseManagement.Services.StatisticsAndReports;
using CaseManagement.Services.Tasks;
using CaseManagement.Services.TimeZoneRegions;
using CaseManagement.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CaseManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            // DcmOpsMonitoring table data auto processor
            services.AddHostedService<ProcessorRunner>();
            services.AddScoped<IScopedProcessingService, Processor>();

            // Application services
            services.AddTransient<ICasesService, CasesService>();
            services.AddTransient<ICaseStatusesService, CaseStatusesService>();
            services.AddTransient<ICasePrioritiesService, CasePrioritiesService>();
            services.AddTransient<ITasksService, TasksService>();
            services.AddTransient<IAnnouncementsService, AnnouncementsService>();
            services.AddTransient<IAgentsStatisticsAndReportsService, AgentsStatisticsAndReportsService>();
            services.AddTransient<CasesTableInputToOutputModelService>();
            services.AddTransient<IDatacentersService, DatacentersService>();
            services.AddTransient<IDateTimeConverterService, DateTimeConverterService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITimeZoneRegionsService, TimeZoneRegionsService>();
            services.AddTransient<IMonitoringService, MonitoringService>();
            services.AddTransient<IAgentAssignmentService, AgentAssignmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            dbContext.Database.Migrate();
        }
    }
}
