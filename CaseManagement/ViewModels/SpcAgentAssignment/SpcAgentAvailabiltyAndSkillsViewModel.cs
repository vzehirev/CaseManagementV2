namespace CaseManagement.ViewModels.SpcAgentAssignment
{
    public class SpcAgentAvailabiltyAndSkillsViewModel
    {
        public string UserId { get; set; }

        public string CUser { get; set; }

        public string FullName { get; set; }

        public bool IsAvailable { get; set; }

        public bool Urgent { get; set; }

        public bool Immediate { get; set; }

        public bool Normal { get; set; }

        public bool Low { get; set; }
    }
}
