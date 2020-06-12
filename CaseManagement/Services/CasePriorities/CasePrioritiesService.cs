using CaseManagement.Data;
using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.Cases.Index;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.CasePriorities
{
    public class CasePrioritiesService : ICasePrioritiesService
    {
        private readonly ApplicationDbContext dbContext;

        public CasePrioritiesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CasePriorityViewModel>> GetAllCasePrioritiesAsync()
        {
            var result = await dbContext.CasePriorities
                    .Select(cp => new CasePriorityViewModel
                    {
                        Id = cp.Id,
                        CasePriorityName = cp.CasePriorityName,
                    })
                    .ToArrayAsync();

            return result;
        }
    }
}
