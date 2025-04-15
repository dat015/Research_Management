using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class progress_report_dto
    {
        public int TopicId { get; set; }

        public DateTime ReportDate { get; set; }
        public int MilestoneId { get; set; } // Liên kết với mốc tiến độ

        public string Description { get; set; }

        public string? FilePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? ProgressPercentage { get; set; } 
        public decimal? UsedAmount { get; set; }

    }
}