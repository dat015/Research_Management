using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class milestone_dto
    {
        public int? MilestoneID { get; set; }
        public int TopicId { get; set; }
        public string Description { get; set; }
        
        public DateTime DueDate { get; set; }
        public DateTime? EndDate { get; set; } // Nullable vì có thể chưa hoàn thành
        public string Status { get; set; } // "pending", "in_progress", "completed"
        public DateTime? CompletedDate { get; set; }= null; // Nullable vì có thể chưa hoàn thành
        public decimal ProgressPercentage { get; set; }
    }
}