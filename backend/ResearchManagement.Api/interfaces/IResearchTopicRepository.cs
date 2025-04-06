using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.dtos;

namespace ResearchManagement.Api.interfaces
{
    public interface IResearchTopicRepository
    {
        Task<bool> CreateResearchTopic(research_topic_dto researchTopicDto);
        Task<bool> UpdateResearchTopic(int id, research_topic_dto researchTopicDto);
        Task<bool> DeleteResearchTopic(int id);
        Task<research_topic_dto> GetResearchTopicById(int id);
        Task<List<research_topic_dto>> GetAllResearchTopics();
        Task<List<research_topic_dto>> GetResearchTopicsByUserId(int userId);
    }
}