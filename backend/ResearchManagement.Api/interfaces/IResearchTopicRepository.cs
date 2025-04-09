using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.interfaces
{
    public interface IResearchTopicRepository
    {
        Task<bool> CreateResearchTopic(research_topic_dto researchTopicDto);
        Task<bool> UpdateResearchTopic(int id, research_topic_dto researchTopicDto);
        Task<bool> DeleteResearchTopic(int id);
        Task<research_topic_dto> GetResearchTopicById(int id);
        Task<List<research_topic_dto>> GetAllResearchTopics();
        Task<List<ResearchTopic>> GetResearchTopicsByUserId(int userId);
        Task<List<dynamic>> getApprovedButNotCompletedRecords(int lectureId);
        Task<bool> reportProgress(progress_report_dto progressReportDto);
    }
}