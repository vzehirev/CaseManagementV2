﻿namespace CaseManagement.ViewModels.SnAgentAssignment
{
    public class SnAgentAvailabiltyAndSkillsViewModel
    {
        public string UserId { get; set; }

        public string CUser { get; set; }

        public string FullName { get; set; }

        public bool IsAvailable { get; set; }

        public bool TTES12 { get; set; }

        public bool TTES34 { get; set; }

        public bool RTPU12 { get; set; }

        public bool RTPU34 { get; set; }

        public bool EX12 { get; set; }

        public bool EX34 { get; set; }

        public bool Other12 { get; set; }

        public bool Other34 { get; set; }
    }
}
