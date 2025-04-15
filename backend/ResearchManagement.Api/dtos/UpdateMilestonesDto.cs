using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class UpdateMilestonesDto
    {
        public List<MilestoneDto> Milestones { get; set; }
    }
}