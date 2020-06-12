using CaseManagement.Data;
using CaseManagement.Models.CaseModels;
using CaseManagement.ViewModels.Cases.Index;
using CaseManagement.ViewModels.CaseStatuses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Services.CaseStatuses
{
    public class CaseStatusesService : ICaseStatusesService
    {
        private readonly ApplicationDbContext dbContext;

        public CaseStatusesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CaseStatusViewModel>> GetAllCaseStatusesAsync()
        {
            var result = await dbContext.CaseStatuses
                    .Select(cp => new CaseStatusViewModel
                    {
                        Id = cp.Id,
                        CaseStatusName = cp.CaseStatusName,
                    })
                    .ToArrayAsync();

            return result;
        }
    }
}
