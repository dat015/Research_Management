using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class SubmitProgressReportDto
    {
            public int TopicId { get; set; }
        public DateTime ReportDate { get; set; }
        public string Description { get; set; }
        public int ProgressPercentage { get; set; }
        public decimal UsedAmount { get; set; }
        public IFormFile File { get; set; }
        public List<MilestoneDto_1> Milestones { get; set; }
        public List<IssueDto_1> Issues { get; set; }
    }
}