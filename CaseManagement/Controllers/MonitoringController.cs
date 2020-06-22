using CaseManagement.Services.Monitoring;
using CaseManagement.ViewModels.Monitoring;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CaseManagement.Controllers
{
    public class MonitoringController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}