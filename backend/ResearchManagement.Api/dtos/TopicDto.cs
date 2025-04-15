using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class TopicDto
    {
        public int TopicId { get; set; }
        public string Title { get; set; }
        public int? CurrentProgress { get; set; }
        public List<MilestoneDto_1> Milestones { get; set; }
        public List<IssueDto_1> Issues { get; set; }
    }

    public class MilestoneDto_1
    {
        public int MilestoneId { get; set; }
        public int TopicId { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? ProgressPercentage { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? Status { get; set; }
    }

    public class IssueDto_1
    {
        public int IssueId { get; set; }
        public int TopicId { get; set; }
        public string? Description { get; set; }
        public string? Impact { get; set; }
        public string? Status { get; set; }
        public string? Resolution { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}