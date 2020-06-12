using CaseManagement.ViewModels.CasePriorities;
using CaseManagement.ViewModels.CaseStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.ViewModels.Cases.Index
{
    public class CasesIndexViewModel
    {
        public string Announcements { get; set; }

        public IEnumerable<CaseViewModel> Cases { get; set; }

        public int CasesCount { get; set; }

        public int PageNumber { get; set; }

        public int LastPage { get; set; }

        public string OrderedBy { get; set; }

        public IEnumerable<CaseStatusViewModel> AllCaseStatuses { get; set; }

        public IEnumerable<CasePriorityViewModel> AllCasePriorities { get; set; }

        public IEnumerable<int> SelectedStatuses { get; set; }

        public IEnumerable<int> SelectedPriorities { get; set; }
    }
}
