using CaseManagement.ViewModels.Cases.Index;
using CaseManagement.ViewModels.CaseStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.CaseStatuses
{
    public interface ICaseStatusesService
    {
        public Task<IEnumerable<CaseStatusViewModel>> GetAllCaseStatusesAsync();
    }
}
