using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.data;
using ResearchManagement.Api.dtos;

namespace ResearchManagement.Api.interfaces
{
    public interface IFinalReportRepository
    {
        Task<List<FinalReportDto>> GetPendingFinalReports();
        Task<DetailFinalReportDTO> GetDetailFinalReportDTOs(int FinalReportId);
        Task<bool> AcceptFinalReportTopic(int TopicId);
        Task<bool> RejectFinalReportTopic(int TopicId);

    }
}