using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.interfaces;

namespace ResearchManagement.Api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetRepository _budgetRepository;
        public BudgetController(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository ?? throw new ArgumentNullException(nameof(budgetRepository));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBudget([FromBody] budget_dto budget)
        {
            if (budget == null)
            {
                return BadRequest("Budget data is required.");
            }
            try
            {
                var createdBudget = await _budgetRepository.CreateBudget(budget);
                return Ok(createdBudget);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("summary/{topicId}")]
        public async Task<IActionResult> GetBudgetSummary(int topicId)
        {
            if (topicId <= 0)
            {
                return BadRequest("Invalid topic ID.");
            }
            try
            {
                var budgetSummary = await _budgetRepository.GetBudgetSummary(topicId);
                if (budgetSummary == null)
                {
                    return NotFound($"No budget found for topic ID {topicId}.");
                }
                return Ok(budgetSummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBudget([FromBody] budget_dto budget)
        {
            if (budget == null)
            {
                return BadRequest("Budget data is required.");
            }
            try
            {
                var updatedBudget = await _budgetRepository.UpdateBudget(budget);
                return Ok(updatedBudget);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}