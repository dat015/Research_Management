using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class MilestoneDto
    {
        public int? MilestoneId { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
    }
}