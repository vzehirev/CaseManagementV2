using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.Cases.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.CasePriorities
{
    public interface ICasePrioritiesService
    {
        public Task<IEnumerable<CasePriorityViewModel>> GetAllCasePrioritiesAsync();
    }
}
