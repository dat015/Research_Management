using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class CreateProgressReportDto
    {
        public DateTime ReportDate { get; set; }
        public string Description { get; set; }
        public string? FilePath { get; set; }
        public decimal? UsedAmount { get; set; }
        public List<ProgressReportIssueDto> Issues { get; set; }
    }
    public class ProgressReportIssueDto
    {
        public string Description { get; set; }
        public string Impact { get; set; }
        public string Status { get; set; }
        public string? Resolution { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}