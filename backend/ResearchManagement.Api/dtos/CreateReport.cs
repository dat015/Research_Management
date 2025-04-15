using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class CreateReport
    {
        public progress_report_dto ProgressReport { get; set; } = new progress_report_dto();
        public List<IssueDto> Issues { get; set; } = new List<IssueDto>();
    }
}