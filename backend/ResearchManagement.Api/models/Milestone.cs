using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class Milestone
    {
        [Key]
        public int MilestoneId { get; set; }

        [Required]
        public int TopicId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal ProgressPercentage { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string Status { get; set; } // "pending", "in_progress", "completed"
        [ForeignKey("TopicId")]

        public ResearchTopic ResearchTopic { get; set; }
        public ProgressReport ProgressReport { get; set; } // Một mốc chỉ có một báo cáo
    }
}