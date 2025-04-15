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

namespace ResearchManagement.Api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressReportController : ControllerBase
    {
        readonly ApplicationDbContext _context;
        private readonly IResearchTopicRepository _researchTopicRepository;
        public ProgressReportController(ApplicationDbContext context, IResearchTopicRepository researchTopicRepository)
        {
            _researchTopicRepository = researchTopicRepository;
            _context = context;
        }



        [HttpPost("createReport")]
        public async Task<IActionResult> createReport([FromForm] CreateReport dto, IFormFile file)
        {
            if (dto == null || dto.ProgressReport == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                dto.ProgressReport.FilePath = filePath;
            }

            try
            {
                var result = await _researchTopicRepository.AddProgressReport(dto);
                if (result)
                {
                    return Ok(new { message = "Báo cáo tiến độ đã được tạo thành công." });
                }
                else
                {
                    return BadRequest("Không thể tạo báo cáo tiến độ.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

        [HttpPost("create-for-milestone/{milestoneId}")]
        public async Task<IActionResult> CreateProgressReportForMilestone(int milestoneId, [FromBody] CreateProgressReportDto dto)
        {
            // Kiểm tra xem mốc có tồn tại không
            var milestone = await _context.Milestones
                .Include(m => m.ProgressReport)
                .FirstOrDefaultAsync(m => m.MilestoneId == milestoneId);

            if (milestone == null)
            {
                return NotFound("Mốc tiến độ không tồn tại.");
            }

            // Kiểm tra xem mốc đã có báo cáo chưa
            if (milestone.ProgressReport != null)
            {
                return BadRequest("Mốc tiến độ này đã được báo cáo.");
            }

            // Tạo báo cáo mới
            var report = new ProgressReport
            {
                TopicId = milestone.TopicId,
                MilestoneId = milestoneId,
                ReportDate = dto.ReportDate,
                Description = dto.Description,
                FilePath = dto.FilePath,
                usedAmount = dto.UsedAmount,
                CreatedAt = DateTime.Now,
                Issues = dto.Issues.Select(i => new ProgressReportIssue
                {
                    Description = i.Description,
                    Impact = i.Impact,
                    Status = i.Status,
                    Resolution = i.Resolution,
                    CreatedAt = DateTime.Now,
                    ResolvedAt = i.ResolvedAt
                }).ToList()
            };

            _context.ProgressReports.Add(report);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Báo cáo tiến độ đã được tạo thành công.", reportId = report.ReportId });
        }

        [HttpGet("by-milestone/{milestoneId}")]
        public async Task<IActionResult> GetProgressReportByMilestone(int milestoneId)
        {
            var report = await _context.ProgressReports
                .Include(r => r.Issues)
                .FirstOrDefaultAsync(r => r.MilestoneId == milestoneId);

            if (report == null)
            {
                return NotFound("Chưa có báo cáo cho mốc này.");
            }

            return Ok(report);
        }


    }
}