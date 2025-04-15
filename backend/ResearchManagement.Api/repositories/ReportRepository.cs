using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.interfaces;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.repositories
{
    public class ReportRepository : IReportRepository
    {
        public async Task<List<ProgressReport>> GetSubmissionHistoryAsync(int topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProgressReport> SubmitProgressReportAsync(int topicId, string description, DateTime reportDate, IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}