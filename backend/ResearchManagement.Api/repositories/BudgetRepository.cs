using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResearchManagement.Api.data;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.interfaces;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;
        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Budget> CreateBudget(budget_dto budget)
        {
            if (budget == null)
            {
                throw new ArgumentNullException(nameof(budget));
            }
            try
            {
                var existingBudget = await _context.Budgets.FirstOrDefaultAsync(b => b.TopicId == budget.TopicId);
                if (existingBudget != null)
                {
                    throw new InvalidOperationException($"Budget already exists for topic ID {budget.TopicId}");
                }
                var newBudget = new Budget
                {
                    TopicId = budget.TopicId,
                    AllocatedAmount = budget.AllocatedAmount,
                    UsedAmount = budget.UsedAmount,
                    UpdatedAt = DateTime.Now
                };
                await _context.Budgets.AddAsync(newBudget);
                await _context.SaveChangesAsync();
                return newBudget;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating budget", ex);
            }
        }

        public async Task<budget_dto> GetBudgetSummary(int topicId)
        {
            if (topicId <= 0)
            {
                throw new ArgumentException("Invalid topic ID", nameof(topicId));
            }
            try
            {
                var budget = await _context.Budgets.Where(b => b.TopicId == topicId)
                    .Select(b => new budget_dto
                    {
                        TopicId = b.TopicId,
                        AllocatedAmount = b.AllocatedAmount,
                        UsedAmount = b.UsedAmount,
                        UpdatedAt = b.UpdatedAt
                    })
                    .FirstOrDefaultAsync();
                if (budget == null)
                {
                    throw new KeyNotFoundException($"Budget not found for topic ID {topicId}");
                }
                return budget;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting budget summary", ex);
            }
        }

        public async Task<budget_dto> UpdateBudget(budget_dto budget)
        {
            if (budget == null)
            {
                throw new ArgumentNullException(nameof(budget));
            }
            try
            {
                var existingBudget = _context.Budgets.FirstOrDefault(b => b.TopicId == budget.TopicId);
                if (existingBudget == null)
                {
                    throw new KeyNotFoundException($"Budget not found for topic ID {budget.TopicId}");
                }
                existingBudget.AllocatedAmount = budget.AllocatedAmount;
                existingBudget.UsedAmount = budget.UsedAmount;
                existingBudget.UpdatedAt = DateTime.Now;
                _context.Budgets.Update(existingBudget);
                await _context.SaveChangesAsync();
                return new budget_dto
                {
                    TopicId = existingBudget.TopicId,
                    AllocatedAmount = existingBudget.AllocatedAmount,
                    UsedAmount = existingBudget.UsedAmount,
                    UpdatedAt = existingBudget.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating budget", ex);
            }
        }

       
    }
}