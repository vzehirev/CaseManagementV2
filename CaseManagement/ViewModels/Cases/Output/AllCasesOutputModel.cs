using CaseManagement.ViewModels.CasePriorities.Output;
using CaseManagement.ViewModels.CaseStatuses.Output;
using System.Collections;
using System.Collections.Generic;

namespace CaseManagement.ViewModels.Cases.Output
{
    public class AllCasesOutputModel
    {
        public int AllCases { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public CaseOutputModel[] Cases { get; set; }

        public string OrderedBy { get; set; }

        public string Announcements { get; set; }

        public CaseStatusOuputModel[] AllAvailableCaseStatuses { get; set; }

        public IEnumerable<int> SelectedStatuses { get; set; }

        public IEnumerable<int> SelectedPriorities { get; set; }

        public CasePriorityOutputModel[] AllAvailableCasePriorities { get; set; }
    }
}
