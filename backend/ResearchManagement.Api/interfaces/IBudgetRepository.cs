using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget> CreateBudget(budget_dto budget);
        Task<budget_dto> UpdateBudget(budget_dto budget);
        Task<budget_dto> GetBudgetSummary(int topicId);

    }
}