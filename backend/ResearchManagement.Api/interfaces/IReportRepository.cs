using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.interfaces
{
    public interface IReportRepository
    {
        Task<ProgressReport> SubmitProgressReportAsync(int topicId, string description, DateTime reportDate, IFormFile file);
        Task<List<ProgressReport>> GetSubmissionHistoryAsync(int topicId);
    }
}