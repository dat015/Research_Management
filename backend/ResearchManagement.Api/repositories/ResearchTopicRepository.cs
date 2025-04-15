using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ResearchManagement.Api.data;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.interfaces;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.repositories
{
    public class ResearchTopicRepository : IResearchTopicRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath;
        public ResearchTopicRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _uploadPath = configuration["UploadPath"] ?? "wwwroot/uploads";
        }
        // This method is not implemented yet. It should create a new research topic in the database.
        public async Task<bool> CreateResearchTopic(research_topic_dto researchTopicDto)
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
                await _context.SaveChangesAsync();
                await AssignReviewersAutomatically(researchTopic.TopicId);
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error creating research topic", ex);
            }
        }
        private async Task AssignReviewersAutomatically(int topicId)
        {
            try
            {
                var topic = await _context.ResearchTopics.FindAsync(topicId);
                if (topic == null)
                {
                    throw new InvalidOperationException("Research topic not found.");
                }
                var councilMembers = await _context.Users
                    .Where(c => c.Role == "council" && !c.isSeniorCouncilMember)
                    .ToListAsync();

                if (councilMembers == null)
                {
                    throw new Exception("Not found Council Member");
                }

                // Lọc Council Members theo department
                var matchingMembers = councilMembers
                    .Where(cm => cm.Department == topic.Field)
                    .ToList();
                var remainingMembers = councilMembers
                    .Where(cm => cm.Department != topic.Field)
                    .ToList();

                //phân chia 3 người xét duyệt
                const int requiredReviewers = 3;
                var assignedReviewers = new List<User>();
                // Ưu tiên chọn từ department khớp
                // Chọn người có lượng xét duyệt topic thấp để ưu tiên
                assignedReviewers.AddRange(matchingMembers
                    .OrderBy(cm => _context.TopicReviewAssignments.Count(a => a.ReviewerId == cm.UserId && !a.HasReviewed))
                    .Take(Math.Min(requiredReviewers, matchingMembers.Count)));

                // Nếu chưa đủ, chọn thêm từ các department khác
                if (assignedReviewers.Count < requiredReviewers)
                {
                    assignedReviewers.AddRange(remainingMembers
                        .OrderBy(cm => _context.TopicReviewAssignments.Count(a => a.ReviewerId == cm.UserId && !a.HasReviewed))
                        .Take(requiredReviewers - assignedReviewers.Count));
                }

                // Lưu phân công
                foreach (var reviewer in assignedReviewers)
                {
                    _context.TopicReviewAssignments.Add(new TopicReviewAssignment
                    {
                        TopicId = topicId,
                        ReviewerId = reviewer.UserId,
                        HasReviewed = false
                    });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error assigning reviewers", ex);
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

        public async Task<bool> reportProgress(progress_report_dto progressReportDto)
        {
            if (progressReportDto == null)
            {
                throw new ArgumentNullException(nameof(progressReportDto));
            }

            var topic = await _context.ResearchTopics
                .Include(t => t.ProgressReports)
                .FirstOrDefaultAsync(t => t.TopicId == progressReportDto.TopicId);

            if (topic == null)
            {
                throw new InvalidOperationException("Đề tài không tồn tại.");
            }
            var totalUsedAmount = topic.ProgressReports?.Sum(r => r.usedAmount) ?? 0;
            if (totalUsedAmount + progressReportDto.UsedAmount > topic.Budget)
            {
                throw new InvalidOperationException("Kinh phí vượt quá ngân sách tổng!");
            }

            // Map the DTO to the entity model
            var progressReport = new ProgressReport
            {
                TopicId = progressReportDto.TopicId,
                ReportDate = progressReportDto.ReportDate,
                Description = progressReportDto.Description,
                FilePath = progressReportDto.FilePath,
                CreatedAt = DateTime.Now,
                usedAmount = progressReportDto.UsedAmount,
                MilestoneId = progressReportDto.MilestoneId // Liên kết với mốc tiến độ
            };

            var milestone = await _context.Milestones
                .Where(m => m.TopicId == progressReportDto.TopicId && m.EndDate == null)
                .FirstOrDefaultAsync();

            if (milestone != null)
            {
                if (progressReportDto.ProgressPercentage < milestone.ProgressPercentage)
                {
                    throw new InvalidOperationException("Tiến độ mới không thể thấp hơn tiến độ hiện tại.");
                }
                // Cập nhật milestone hiện tại
                milestone.ProgressPercentage = progressReportDto.ProgressPercentage ?? 0;
                milestone.Description = progressReportDto.Description; // Cập nhật mô tả nếu cần
                if (progressReportDto.ProgressPercentage == 100)
                {
                    milestone.EndDate = DateTime.Now; // Đánh dấu hoàn thành
                }
                _context.Milestones.Update(milestone);
            }
            else
            {
                // Tạo milestone mới nếu không có milestone chưa hoàn thành
                milestone = new Milestone
                {
                    TopicId = progressReportDto.TopicId,
                    Description = progressReportDto.Description,
                    DueDate = DateTime.Now.AddDays(30),
                    ProgressPercentage = progressReportDto.ProgressPercentage ?? 0,
                    EndDate = progressReportDto.ProgressPercentage == 100 ? DateTime.Now : null,

                    // Không cần gán MilestoneID
                };
                await _context.Milestones.AddAsync(milestone);
                await _context.SaveChangesAsync();
            }
            try
            {
                await _context.ProgressReports.AddAsync(progressReport);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // You can log the error here if needed
                throw new Exception("Error creating progress report", ex);
            }
        }

        public async Task<List<milestone_dto>> GetMilestonesByTopicIdAsync(int topicId)
        {
            if (topicId <= 0)
            {
                throw new ArgumentException("Invalid topic ID", nameof(topicId));
            }

            try
            {
                var milestones = await _context.Milestones
                    .Where(m => m.TopicId == topicId)
                    .Select(m => new milestone_dto
                    {
                        MilestoneID = m.MilestoneId,
                        TopicId = m.TopicId,
                        Description = m.Description,
                        DueDate = m.DueDate,
                        CompletedDate = m.EndDate,
                        ProgressPercentage = m.ProgressPercentage,
                        Status = m.Status,
                        EndDate = m.EndDate

                    })
                    .ToListAsync();
                return milestones;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error retrieving milestones", ex);
            }
        }

        public async Task<List<topic_pending_dto>> GetPendingTopics()
        {
            try
            {
                var topics = await _context.ResearchTopics
                    .Include(t => t.User)
                    .Include(t => t.Reviews).ThenInclude(r => r.User)
                    .Include(t => t.TopicReviewAssignments).ThenInclude(a => a.Reviewer)
                    .Where(t => t.Status == "Pending" || t.Status == "NeedsRevision")
                    .Select(t => new topic_pending_dto
                    {
                        TopicId = t.TopicId,
                        Title = t.Title,
                        Lecturer = t.User.FullName,
                        Field = t.Field,
                        Status = t.Status,
                        CouncilFeedback = t.CouncilFeedback,
                        ReviewCount = t.TopicReviewAssignments.Count(a => a.HasReviewed),
                        TotalReviewers = t.TopicReviewAssignments.Count(),
                        IsReviewComplete = t.TopicReviewAssignments.Any() && t.TopicReviewAssignments.All(a => a.HasReviewed),
                        AssignedReviewers = t.TopicReviewAssignments
                            .Select(a => new ReviewerAssignmentDto
                            {
                                ReviewerId = (int)a.ReviewerId,
                                FullName = a.Reviewer.FullName,
                                Department = a.Reviewer.Department,
                                HasReviewed = a.HasReviewed,
                                Evaluation = a.HasReviewed ? new EvaluationDto
                                {
                                    Feasibility = (int)t.Reviews.FirstOrDefault(r => r.CouncilMemberId == a.ReviewerId).FeasibilityScore,
                                    Novelty = (int)t.Reviews.FirstOrDefault(r => r.CouncilMemberId == a.ReviewerId).NoveltyScore,
                                    Applicability = (int)t.Reviews.FirstOrDefault(r => r.CouncilMemberId == a.ReviewerId).ApplicabilityScore,
                                    Total = t.Reviews.FirstOrDefault(r => r.CouncilMemberId == a.ReviewerId).TotalScore
                                } : null,
                                Feedback = a.HasReviewed ? t.Reviews.FirstOrDefault(r => r.CouncilMemberId == a.ReviewerId).Comments : null,
                                ReviewedAt = a.HasReviewed ? t.Reviews.FirstOrDefault(r => r.CouncilMemberId == a.ReviewerId).CreatedAt : null
                            }).ToList(),
                        RemainingReviewers = t.TopicReviewAssignments
                            .Where(a => !a.HasReviewed)
                            .Select(a => new ReviewerDto
                            {
                                ReviewerId = (int)a.ReviewerId,
                                FullName = a.Reviewer.FullName,
                                Department = a.Reviewer.Department
                            }).ToList(),
                        ReviewDate = t.Reviews.Any() ? t.Reviews.Max(r => r.CreatedAt) : null,
                        Evaluation = t.Reviews.Any() ? new EvaluationDto
                        {
                            Feasibility = (int)t.Reviews.Average(r => r.FeasibilityScore),
                            Novelty = (int)t.Reviews.Average(r => r.NoveltyScore),
                            Applicability = (int)t.Reviews.Average(r => r.ApplicabilityScore),
                            Total = t.Reviews.Average(r => r.TotalScore)
                        } : null,
                        RevisionDeadline = t.Status == "NeedsRevision" ? t.UpdatedAt.AddDays(7) : null
                    })
                    .ToListAsync();
                return topics;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving pending topics", ex);
            }
        }

        public async Task<List<TopicDto>> GetApprovedButNotCompletedRecords(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "lecturer");

            if (user == null)
            {
                throw new Exception("Người dùng không phải là giảng viên");
            }

            var topics = await _context.ResearchTopics
                .Include(t => t.Milestones)
                .Include(t => t.Issues)
                .Where(t => t.UserId == user.UserId && t.Status == "Approved" && (t.CurrentProgress < 100 || t.CurrentProgress == null))
                .ToListAsync();

            return topics.Select(t => new TopicDto
            {
                TopicId = t.TopicId,
                Title = t.Title,
                CurrentProgress = t.CurrentProgress,
                Milestones = t.Milestones.Select(m => new MilestoneDto_1
                {
                    MilestoneId = m.MilestoneId,
                    TopicId = m.TopicId,
                    Description = m.Description,
                    DueDate = m.DueDate,
                    EndDate = (DateTime)m.EndDate,
                    ProgressPercentage = m.ProgressPercentage,
                    CompletionDate = m.CompletionDate,
                    Status = m.Description
                }).ToList(),
                Issues = t.Issues.Select(i => new IssueDto_1
                {
                    IssueId = i.IssueId,
                    TopicId = i.TopicId,
                    Description = i.Description,
                    Impact = i.Impact,
                    Status = i.Status,
                    Resolution = i.Resolution,
                    CreatedAt = i.CreatedAt,
                    ResolvedAt = i.ResolvedAt
                }).ToList()
            }).ToList();
        }

        public async Task<budget_dto> GetBudgetSummary(int userId, int topicId)
        {
            var user = await _context.Users
                 .FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "lecturer");

            if (user == null)
            {
                throw new Exception("Người dùng không phải là giảng viên");
            }

            var topic = await _context.ResearchTopics
                .Include(t => t.BudgetDetail)
                .FirstOrDefaultAsync(t => t.TopicId == topicId && t.UserId == user.UserId);

            if (topic == null)
            {
                throw new Exception("Không tìm thấy đề tài hoặc bạn không có quyền truy cập");
            }

            var budget = topic.BudgetDetail ?? new Budget
            {
                TopicId = topic.TopicId,
                AllocatedAmount = topic.Budget,
                UsedAmount = 0,
                UpdatedAt = DateTime.Now
            };

            return new budget_dto
            {
                TopicId = topic.TopicId,
                AllocatedAmount = budget.AllocatedAmount,
                UsedAmount = budget.UsedAmount,
                UpdatedAt = budget.UpdatedAt
            };
        }

        public async Task SubmitProgressReport(int userId, SubmitProgressReportDto dto)
        {
            var user = await _context.Users
                            .FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "lecturer");

            if (user == null)
            {
                throw new Exception("Người dùng không phải là giảng viên");
            }

            var topic = await _context.ResearchTopics
                .Include(t => t.Milestones)
                .Include(t => t.Issues)
                .Include(t => t.BudgetDetail)
                .FirstOrDefaultAsync(t => t.TopicId == dto.TopicId && t.UserId == user.UserId);

            if (topic == null)
            {
                throw new Exception("Không tìm thấy đề tài hoặc bạn không có quyền truy cập");
            }

            // Lưu file
            if (dto.File == null)
            {
                throw new Exception("File báo cáo là bắt buộc");
            }

            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(_uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Bắt đầu transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Cập nhật tiến độ tổng thể
                topic.CurrentProgress = dto.ProgressPercentage;
                topic.UpdatedAt = DateTime.Now;
                _context.ResearchTopics.Update(topic);

                // Cập nhật kinh phí
                var budget = topic.BudgetDetail ?? new Budget
                {
                    TopicId = topic.TopicId,
                    AllocatedAmount = topic.Budget,
                    UsedAmount = 0,
                    UpdatedAt = DateTime.Now
                };

                budget.UsedAmount += dto.UsedAmount;
                budget.UpdatedAt = DateTime.Now;

                if (topic.BudgetDetail == null)
                {
                    _context.Budgets.Add(budget);
                }
                else
                {
                    _context.Budgets.Update(budget);
                }

                // Cập nhật mốc công việc
                if (dto.Milestones != null)
                {
                    foreach (var milestoneDto in dto.Milestones)
                    {
                        var milestone = topic.Milestones.FirstOrDefault(m => m.MilestoneId == milestoneDto.MilestoneId);
                        if (milestone != null)
                        {
                            milestone.ProgressPercentage = (int)milestoneDto.ProgressPercentage;
                            milestone.Description = milestoneDto.Status;
                            milestone.CompletionDate = milestoneDto.Status == "completed" ? DateTime.Now : null;
                            milestone.EndDate = DateTime.Now;
                            _context.Milestones.Update(milestone);
                        }
                    }
                }

                // Cập nhật vấn đề
                if (dto.Issues != null)
                {
                    foreach (var issueDto in dto.Issues)
                    {
                        var issue = topic.Issues.FirstOrDefault(i => i.IssueId == issueDto.IssueId);
                        if (issue != null)
                        {
                            issue.Status = issueDto.Status;
                            issue.Resolution = issueDto.Resolution;
                            issue.ResolvedAt = issueDto.Status == "resolved" ? DateTime.Now : null;
                            _context.Issues.Update(issue);
                        }
                    }
                }

                // Lưu báo cáo tiến độ
                var progressReport = new ProgressReport
                {
                    TopicId = dto.TopicId,
                    ReportDate = dto.ReportDate,
                    Description = dto.Description,
                    FilePath = $"/uploads/{fileName}",
                    usedAmount = dto.UsedAmount,
                    CreatedAt = DateTime.Now
                };
                _context.ProgressReports.Add(progressReport);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<TopicDto>> GetTopicApprovalList(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }
            try
            {
                var topics = await _context.ResearchTopics
                    .Include(t => t.User)
                    .Include(t => t.TopicReviewAssignments).ThenInclude(a => a.Reviewer)
                    .Where(t => t.Status != "Pending" && t.Status != "NeedsRevision" && t.Status != "Completed" && t.UserId == userId)
                    .Select(t => new TopicDto
                    {
                        TopicId = t.TopicId,
                        Title = t.Title,
                        CurrentProgress = t.CurrentProgress,
                        Milestones = t.Milestones.Select(m => new MilestoneDto_1
                        {
                            MilestoneId = m.MilestoneId,
                            TopicId = m.TopicId,
                            Description = m.Description,
                            DueDate = m.DueDate,
                            EndDate = (DateTime)m.EndDate,
                            ProgressPercentage = m.ProgressPercentage,
                            CompletionDate = m.CompletionDate,
                            Status = m.Description
                        }).ToList(),
                        Issues = t.Issues.Select(i => new IssueDto_1
                        {
                            IssueId = i.IssueId,
                            TopicId = i.TopicId,
                            Description = i.Description,
                            Impact = i.Impact,
                            Status = i.Status,
                            Resolution = i.Resolution,
                            CreatedAt = i.CreatedAt,
                            ResolvedAt = i.ResolvedAt
                        }).ToList()
                    })
                    .ToListAsync();
                return topics;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving topic approval list", ex);
            }


        }

        public Task<bool> AddMilestones(int topicId, List<milestone_dto> milestoneDto)
        {
            if (milestoneDto == null)
            {
                throw new ArgumentNullException(nameof(milestoneDto));
            }

            // Find the research topic by ID
            var researchTopic = _context.ResearchTopics.Find(topicId);
            if (researchTopic == null)
            {
                return Task.FromResult(false); // Topic not found
            }

            for (int i = 0; i < milestoneDto.Count - 1; i++)
            {
                if (milestoneDto[i].EndDate >= milestoneDto[i + 1].DueDate)
                {
                    throw new Exception("Thời gian không hợp lệ");
                }
            }

            // Map the DTO to the entity model
            var milestones = milestoneDto.Select(m => new Milestone
            {
                TopicId = m.TopicId,
                Description = m.Description,
                DueDate = m.DueDate,
                ProgressPercentage = m.ProgressPercentage,
                EndDate = m.EndDate,
                Status = m.Status
            }).ToList();

            // Add the milestone to the context
            try
            {
                foreach (var milestone in milestones)
                {
                    _context.Milestones.Add(milestone);
                }
                return _context.SaveChangesAsync().ContinueWith(task => task.Result > 0);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error creating milestone", ex);
            }
        }

        public async Task<List<progress_track>> GetProgressTrack(int topicId)
        {
            if (topicId <= 0)
            {
                throw new ArgumentException("ID chủ đề không hợp lệ.", nameof(topicId));
            }

            try
            {
                var progressTrack = await _context.ResearchTopics
                    .Where(t => t.TopicId == topicId)
                    .Include(t => t.Milestones) // Load quan hệ Milestones
                    .Include(t => t.Issues)     // Load quan hệ Issues
                    .Include(t => t.ProgressReports) // Load quan hệ ProgressReports (giả định có)
                    .Select(t => new progress_track
                    {
                        Milestones = t.Milestones.Select(m => new milestone_dto
                        {
                            MilestoneID = m.MilestoneId,
                            TopicId = m.TopicId,
                            Description = m.Description,
                            DueDate = m.DueDate,
                            EndDate = m.EndDate, // Không cần ép kiểu nếu đã là DateTime
                            ProgressPercentage = m.ProgressPercentage,
                            CompletedDate = m.CompletionDate,
                            Status = m.Status // Sửa từ m.Description thành m.Status
                        }).ToList(),
                        Issues = t.Issues.Select(i => new IssueDto // Sửa từ IssueDto_1 thành IssueDto
                        {
                            IssueId = i.IssueId,
                            TopicId = i.TopicId,
                            Description = i.Description,
                            Impact = i.Impact,
                            Status = i.Status,
                            Resolution = i.Resolution,

                        }).ToList(),
                        ProgressReports = t.ProgressReports.Select(pr => new progress_report_dto
                        {
                            TopicId = pr.TopicId,
                            ReportDate = pr.ReportDate,
                            MilestoneId = pr.MilestoneId,
                            Description = pr.Description,
                            FilePath = pr.FilePath,
                            CreatedAt = pr.CreatedAt,
                            ProgressPercentage = 0,
                            UsedAmount = pr.usedAmount // Sửa từ pr.UsedAmount thành pr.usedAmount


                        }).ToList()
                    })
                    .ToListAsync();

                if (!progressTrack.Any())
                {
                    throw new KeyNotFoundException($"Không tìm thấy chủ đề với ID {topicId}.");
                }

                return progressTrack;
            }
            catch (Exception ex)
            {
                // Giả định có _logger được inject
                //_logger.LogError(ex, "Lỗi khi lấy tiến độ cho topicId {TopicId}", topicId);
                throw new Exception($"Lỗi khi lấy tiến độ cho topicId {topicId}", ex);
            }
        }

        public async Task<bool> AddProgressReport(CreateReport progressReportDto)
        {
            if (progressReportDto == null)
            {
                throw new ArgumentNullException(nameof(progressReportDto));
            }

            // Find the research topic by ID
            var researchTopic = _context.ResearchTopics.Find(progressReportDto.ProgressReport.TopicId);
            var milestone = _context.Milestones.Find(progressReportDto.ProgressReport.MilestoneId);
            if (milestone == null)
            {
                throw new Exception("Mốc không tồn tại");
            }
            if (researchTopic == null)
            {
                return false;
            }

            if (milestone.EndDate < DateTime.Now || milestone.DueDate > DateTime.Now)
            {
                throw new Exception("Chưa tạo được báo cáo cho mốc này");
            }

            var existingReports = await _context.ProgressReports
                .Where(pr => pr.TopicId == progressReportDto.ProgressReport.TopicId && pr.MilestoneId == progressReportDto.ProgressReport.MilestoneId)
                .FirstOrDefaultAsync();
            if (existingReports != null)
            {
                throw new Exception("Báo cáo đã tồn tại cho mốc này");
            }
            // Map the DTO to the entity model
            var progressReport = new ProgressReport
            {
                TopicId = progressReportDto.ProgressReport.TopicId,
                MilestoneId = progressReportDto.ProgressReport.MilestoneId,
                ReportDate = progressReportDto.ProgressReport.ReportDate,
                Description = progressReportDto.ProgressReport.Description,
                FilePath = progressReportDto.ProgressReport.FilePath,
                CreatedAt = DateTime.Now,
                usedAmount = progressReportDto.ProgressReport.UsedAmount,
            };
            var listIssue = new List<Issue>();

            if (progressReportDto.Issues != null)
            {
                foreach (var issue in progressReportDto.Issues)
                {
                    var newIssue = new Issue
                    {
                        TopicId = progressReportDto.ProgressReport.TopicId,
                        milestone_id = progressReportDto.ProgressReport.MilestoneId,
                        Description = issue.Description,
                        Impact = issue.Impact,
                        Status = issue.Status,
                        Resolution = issue.Resolution,
                        CreatedAt = DateTime.Now
                    };
                    listIssue.Add(newIssue);

                }
                await _context.Issues.AddRangeAsync(listIssue);
                await _context.SaveChangesAsync();

            }


            // Add the progress report to the context
            try
            {
                _context.ProgressReports.Add(progressReport);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Error creating progress report", ex);
            }
        }
    }
}
