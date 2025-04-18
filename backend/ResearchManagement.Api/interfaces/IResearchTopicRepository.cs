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
        Task<List<milestone_dto>> GetMilestonesByTopicIdAsync(int topicId);
        Task<List<topic_pending_dto>> GetPendingTopics();
        Task<List<TopicDto>> GetApprovedButNotCompletedRecords(int userId);
        Task<budget_dto> GetBudgetSummary(int userId, int topicId);
        Task SubmitProgressReport(int userId, SubmitProgressReportDto dto);
        Task<List<TopicDto>> GetTopicApprovalList(int userId);
        Task<bool> AddMilestones(int topicId, List<milestone_dto> milestoneDto);
        Task<List<progress_track>> GetProgressTrack(int topicId);
        Task<bool> AddProgressReport(CreateReport progressReportDto);
        


    }
}