using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class DetailFinalReportDTO
    {
        public FinalReportDto finalReportDto {get; set;}
        public List<MilestoneDto> milestoneDtos {get; set;}
        public List<progress_report_dto> progress_Report_Dtos {get; set;}
    }
}