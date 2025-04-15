using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace ResearchManagement.Api.dtos
{
    public class progress_track
    {
        public List<milestone_dto> Milestones { get; set; } = new List<milestone_dto>();
        public List<IssueDto> Issues { get; set; } = new List<IssueDto>();
        public List<progress_report_dto> ProgressReports { get; set; } = new List<progress_report_dto>();
    }
}