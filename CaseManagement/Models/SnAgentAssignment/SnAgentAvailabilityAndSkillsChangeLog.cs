﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CaseManagement.Models.SnAgentAssignment
{
    public class SnAgentAvailabilityAndSkillsChangeLog
    {
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        [Required]
        public string ChangedByUserId { get; set; }
        public ApplicationUser ChangedByUser { get; set; }

        [Required]
        public string ChangedSkill { get; set; }

        [Required]
        public bool OldValue { get; set; }

        [Required]
        public bool NewValue { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
