using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class UpdateProgressDto
    {
        public int CurrentProgress { get; set; } // Tiến độ hiện tại
        public List<MilestoneDto_1>? Milestones { get; set; } // Danh sách mốc tiến độ
        public List<IssueDto_1>? Issues { get; set; } // Danh sách vấn đề
    }
}