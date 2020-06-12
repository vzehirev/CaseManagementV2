using System.Collections;
using System.Collections.Generic;

namespace CaseManagement.ViewModels.Cases.Input
{
    public class CasesIndexInputModel
    {
        public int PageNumber { get; set; }

        public string OrderBy { get; set; }

        public IEnumerable<int> SelectedStatuses { get; set; }

        public IEnumerable<int> SelectedPriorities { get; set; }
    }
}
