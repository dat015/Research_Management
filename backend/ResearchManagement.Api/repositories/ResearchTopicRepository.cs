using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchManagement.Api.data;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.interfaces;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.repositories
{
    public class ResearchTopicRepository : IResearchTopicRepository
    {
        private readonly ApplicationDbContext _context;
        public ResearchTopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // This method is not implemented yet. It should create a new research topic in the database.
        public Task<bool> CreateResearchTopic(research_topic_dto researchTopicDto)
        {
            if (researchTopicDto == null)
            {
                throw new ArgumentNullException(nameof(researchTopicDto));
            }
            if (LimitedResearchTopic(researchTopicDto))
            {
                throw new InvalidOperationException("User has reached the limit of research topics.");
            }
            // Map the DTO to the entity model
            var researchTopic = new ResearchTopic
            {
                UserId = researchTopicDto.UserId,
                Title = researchTopicDto.Title,
                Field = researchTopicDto.Field,
                Objective = researchTopicDto.Objective,
                Method = researchTopicDto.Method,
                StartDate = researchTopicDto.StartDate,
                EndDate = researchTopicDto.EndDate,
                Budget = researchTopicDto.Budget,
                Status = researchTopicDto.Status
            };

            // Add the research topic to the context
            try
            {
                _context.ResearchTopics.Add(researchTopic);
                return _context.SaveChangesAsync().ContinueWith(task => task.Result > 0);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error creating research topic", ex);
            }
        }
        private bool LimitedResearchTopic(research_topic_dto researchTopicDto)
        {
            var existingResearchTopics = _context.ResearchTopics
                .Where(rt => rt.UserId == researchTopicDto.UserId)
                .ToList();
            return existingResearchTopics.Count >= 3; // Example condition
        }
        public Task<bool> DeleteResearchTopic(int id)
        {
            // Find the research topic by ID
            var researchTopic = _context.ResearchTopics.Find(id);
            if (researchTopic == null)
            {
                return Task.FromResult(false); // Topic not found
            }

            // Remove the research topic from the context
            _context.ResearchTopics.Remove(researchTopic);
            return _context.SaveChangesAsync().ContinueWith(task => task.Result > 0);
        }

        public Task<List<research_topic_dto>> GetAllResearchTopics()
        {
            // Retrieve all research topics from the database
            var researchTopics = _context.ResearchTopics.ToListAsync();
            return researchTopics.ContinueWith(task =>
            {
                return task.Result.Select(rt => new research_topic_dto
                {
                    UserId = rt.UserId,
                    Title = rt.Title,
                    Field = rt.Field,
                    Objective = rt.Objective,
                    Method = rt.Method,
                    StartDate = rt.StartDate,
                    EndDate = rt.EndDate,
                    Budget = rt.Budget,
                    Status = rt.Status
                }).ToList();
            });
        }

        public async Task<research_topic_dto> GetResearchTopicById(int id)
        {
            // Find the research topic by ID
            var researchTopic = await _context.ResearchTopics.FindAsync(id);
            if (researchTopic == null)
            {
                return null; // Topic not found
            }

            return new research_topic_dto
            {
                UserId = researchTopic.UserId,
                Title = researchTopic.Title,
                Field = researchTopic.Field,
                Objective = researchTopic.Objective,
                Method = researchTopic.Method,
                StartDate = researchTopic.StartDate,
                EndDate = researchTopic.EndDate,
                Budget = researchTopic.Budget,
                Status = researchTopic.Status
            };
        }

        public Task<List<ResearchTopic>> GetResearchTopicsByUserId(int userId)
        {
            // Retrieve research topics for a specific user
            var researchTopics = _context.ResearchTopics.Where(rt => rt.UserId == userId)
            .Select(rt => new ResearchTopic
            {
                TopicId = rt.TopicId,
                UserId = rt.UserId,
                Title = rt.Title,
                Field = rt.Field,
                Objective = rt.Objective,
                Method = rt.Method,
                StartDate = rt.StartDate,
                EndDate = rt.EndDate,
                Budget = rt.Budget,
                Status = rt.Status
            })
            .ToListAsync();
            return researchTopics.ContinueWith(task =>
            {
                return task.Result.ToList();
            });

        }

        public Task<bool> UpdateResearchTopic(int id, research_topic_dto researchTopicDto)
        {
            if (researchTopicDto == null)
            {
                throw new ArgumentNullException(nameof(researchTopicDto));
            }

            // Find the research topic by ID
            var researchTopic = _context.ResearchTopics.Find(id);
            if (researchTopic == null)
            {
                return Task.FromResult(false); // Topic not found
            }

            // Update the research topic properties
            researchTopic.Title = researchTopicDto.Title;
            researchTopic.Field = researchTopicDto.Field;
            researchTopic.Objective = researchTopicDto.Objective;
            researchTopic.Method = researchTopicDto.Method;
            researchTopic.StartDate = researchTopicDto.StartDate;
            researchTopic.EndDate = researchTopicDto.EndDate;
            researchTopic.Budget = researchTopicDto.Budget;
            researchTopic.Status = researchTopicDto.Status;

            // Save changes to the context
            return _context.SaveChangesAsync().ContinueWith(task => task.Result > 0);
        }

        public async Task<List<dynamic>> getApprovedButNotCompletedRecords(int lectureId)
        {
            if (lectureId <= 0)
            {
                throw new ArgumentException("Invalid lecture ID", nameof(lectureId));
            }

            try
            {
                var approvedTopics = await _context.ResearchTopics
                    .Where(rt => rt.Status != "Pending" && rt.Status != "Completed" && rt.UserId == lectureId && rt.EndDate >= DateTime.Now)
                    .ToListAsync();
                var approvedTopicsDto = approvedTopics.Select(rt => new
                {
                    TopicId = rt.TopicId,
                    Title = rt.Title,

                }).ToList()
                .Select(item => (dynamic)item).ToList();
                return approvedTopicsDto;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error retrieving research topics", ex);
            }
        }

        public Task<bool> reportProgress(progress_report_dto progressReportDto)
        {
            if (progressReportDto == null)
            {
                throw new ArgumentNullException(nameof(progressReportDto));
            }
            // Map the DTO to the entity model
            var progressReport = new ProgressReport
            {
                TopicId = progressReportDto.TopicId,
                ReportDate = progressReportDto.ReportDate,
                Description = progressReportDto.Description,
                FilePath = progressReportDto.FilePath,
                CreatedAt = DateTime.Now
            };
            try
            {
                // Add the progress report to the context
                _context.ProgressReports.Add(progressReport);
                return _context.SaveChangesAsync().ContinueWith(task => task.Result > 0);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error creating progress report", ex);
            }
        }
    }

}