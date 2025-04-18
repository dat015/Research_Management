using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResearchManagement.Api.data;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.interfaces;

namespace ResearchManagement.Api.repositories
{
    public class FinalReportRepository : IFinalReportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public FinalReportRepository(ApplicationDbContext context, ILogger<FinalReportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AcceptFinalReportTopic(int TopicId)
        {
            if (TopicId <= 0)
            {
                throw new Exception("Không tìm thấy dữ liệu");
            }
            try
            {
                var topicFinal = await _context.FinalReports
                    .Where(f => f.TopicId == TopicId)
                    .FirstOrDefaultAsync();
                if (topicFinal == null)
                {
                    throw new Exception("Không tìm thấy final topic với Id: " + TopicId);
                }

                topicFinal.AcceptanceStatus = "Accept";
                _context.FinalReports.Update(topicFinal);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DetailFinalReportDTO> GetDetailFinalReportDTOs(int FinalReportId)
        {
            if (FinalReportId <= 0)
            {
                throw new Exception("ID not valid");
            }
            try
            {
                var finalReport = await _context.FinalReports
                    .Where(f => f.FinalReportId == FinalReportId)
                    .Include(f => f.ResearchTopic)
                        .ThenInclude(rt => rt.Milestones)
                    .Include(f => f.ResearchTopic)
                        .ThenInclude(m => m.ProgressReports)
                    .Include(f => f.ResearchTopic)
                        .ThenInclude(f => f.User)
                    .Select(f => new DetailFinalReportDTO
                    {
                        milestoneDtos = f.ResearchTopic.Milestones
                            .Select(m => new MilestoneDto
                            {
                                MilestoneId = m.MilestoneId,
                                Description = m.Description,
                                CompletionDate = m.CompletionDate,
                                DueDate = m.DueDate,
                                EndDate = m.EndDate,
                                Status = m.Status,
                                ProgressPercentage = m.ProgressPercentage

                            }).ToList(),
                        progress_Report_Dtos = f.ResearchTopic.ProgressReports
                            .Select(f => new progress_report_dto
                            {
                                TopicId = f.TopicId,
                                ReportDate = f.ReportDate,
                                MilestoneId = f.MilestoneId,
                                Description = f.Description,
                                FilePath = f.FilePath,
                                CreatedAt = f.CreatedAt,
                                UsedAmount = f.usedAmount,
                            })
                            .ToList(),
                        finalReportDto = new FinalReportDto
                        {
                            FinalReportId = f.FinalReportId,
                            TopicId = f.TopicId,
                            Summary = f.Summary,
                            FilePath = f.FilePath,
                            CreatedAt = f.CreatedAt,
                            TopicTitle = f.ResearchTopic.Title,
                            Lecturer = f.ResearchTopic.User.FullName,
                            Department = f.ResearchTopic.User.Department ?? ""
                        }
                    })
                    .FirstOrDefaultAsync();

                return finalReport ?? new DetailFinalReportDTO();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<FinalReportDto>> GetPendingFinalReports()
        {
            try
            {
                var finalReports = await _context.FinalReports
                    .Where(f => f.AcceptanceStatus == "Pending")
                    .Include(f => f.ResearchTopic)
                        .ThenInclude(f => f.User)
                    .Select(f => new FinalReportDto
                    {
                        FinalReportId = f.FinalReportId,
                        TopicId = f.TopicId,
                        FilePath = f.FilePath,
                        CreatedAt = f.CreatedAt,
                        TopicTitle = f.ResearchTopic.Title,
                        Department = f.ResearchTopic.User.Department ?? "",
                        Lecturer = f.ResearchTopic.User.FullName,
                        email = f.ResearchTopic.User.Email
                    })
                    .ToListAsync();

                return (finalReports != null) ? finalReports : new List<FinalReportDto>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> RejectFinalReportTopic(int TopicId)
        {
            if (TopicId <= 0)
            {
                throw new Exception("Không tìm thấy dữ liệu");
            }
            try
            {
                Console.WriteLine("Id : " + TopicId);
                var topic = await _context.ResearchTopics.FirstOrDefaultAsync(t => t.TopicId == TopicId);
                if (topic == null)
                {
                    throw new Exception("Không tìm thấy topic");

                }
                topic.Status = "revision_required";
                _context.ResearchTopics.Update(topic);
                await _context.SaveChangesAsync();


                //Lấy mốc cuối cùng và đổi trạng thái là yêu cầu chỉnh sửa
                var milestone = await _context.Milestones
                    .Where(m => m.TopicId == TopicId)
                    .OrderByDescending(m => m.EndDate) // từ lớn đến bé
                    .FirstOrDefaultAsync();
                if (milestone == null)
                {
                    throw new Exception("Không tìm thấy milestone");

                }
                milestone.Status = "revision_required";
                _context.Milestones.Update(milestone);
                await _context.SaveChangesAsync();


                //Cập nhật trạng thái của final topic
                var finalReport = await _context.FinalReports
                    .Where(f => f.TopicId == TopicId)
                    .FirstOrDefaultAsync();
                if (finalReport == null)
                {
                    throw new Exception("Không tìm thấy final report");

                }
                finalReport.AcceptanceStatus = "revision_required";
                _context.FinalReports.Update(finalReport);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}