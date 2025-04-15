using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
    public class ResearchTopicController : ControllerBase
    {
        private readonly IResearchTopicRepository _researchTopicRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ResearchTopicController(IResearchTopicRepository researchTopicRepository, ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _researchTopicRepository = researchTopicRepository;
            _context = context;
        }

        [HttpGet("progress/{topicId}")]
        public async Task<IActionResult> GetProgressTrack(int topicId)
        {
            var progressTrack = await _researchTopicRepository.GetProgressTrack(topicId);
            if (progressTrack == null)
            {
                return NotFound("Không tìm thấy thông tin tiến độ cho đề tài này.");
            }
            return Ok(progressTrack);
        }

        [HttpPost("add-milestones/{topicId}")]
        public async Task<IActionResult> AddMilestones(int topicId, [FromBody] List<milestone_dto> milestoneDtos)
        {
            var topic = await _context.ResearchTopics.FindAsync(topicId);
            if (topic == null)
            {
                return NotFound("Đề tài không tồn tại.");
            }
            try
            {
                await _researchTopicRepository.AddMilestones(topicId, milestoneDtos);
                return Ok(new { message = "Thêm mốc tiến độ thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Thời gian không hợp lệ", error = ex.Message });
            }

        }

        [HttpPut("update-milestones/{topicId}")]
        public async Task<IActionResult> UpdateMilestones(int topicId, [FromBody] UpdateMilestonesDto dto)
        {
            var topic = await _context.ResearchTopics
                .Include(t => t.Milestones)
                .FirstOrDefaultAsync(t => t.TopicId == topicId);

            if (topic == null)
            {
                return NotFound("Đề tài không tồn tại.");
            }

            // Xóa các mốc cũ
            _context.Milestones.RemoveRange(topic.Milestones);

            // Thêm các mốc mới
            topic.Milestones = dto.Milestones.Select(m => new Milestone
            {
                TopicId = topicId,
                Description = m.Description,
                DueDate = m.DueDate,
                EndDate = m.EndDate,
                ProgressPercentage = m.ProgressPercentage,
                CompletionDate = m.CompletionDate,
                Status = m.Status
            }).ToList();

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật mốc tiến độ thành công." });
        }


        [HttpGet("assigned/{id}")]
        public async Task<IActionResult> GetAssignedTopics(int id)
        {
            try
            {
                var topics = await _researchTopicRepository.GetTopicApprovalList(id);
                if (topics == null || !topics.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy đề tài nào." });
                }
                return Ok(topics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("{id}/progress")]
        public async Task<IActionResult> UpdateTopicProgress(int id, [FromBody] UpdateProgressDto dto)
        {

            var topic = await _context.ResearchTopics
                .Include(t => t.Milestones)
                .Include(t => t.Issues)
                .FirstOrDefaultAsync(t => t.TopicId == id);
            if (dto.Milestones != null)
            {
                topic.Milestones.Clear();
                foreach (var milestone in _mapper.Map<List<Milestone>>(dto.Milestones))
                {
                    topic.Milestones.Add(milestone);
                }
            }

            if (dto.Issues != null)
            {
                topic.Issues.Clear();
                foreach (var issue in _mapper.Map<List<Issue>>(dto.Issues))
                {
                    topic.Issues.Add(issue);
                }
            }

            if (topic == null)
                return NotFound(new { Message = "Không tìm thấy đề tài." });

            topic.CurrentProgress = dto.CurrentProgress;
            topic.Milestones.Clear();
            foreach (var milestone in _mapper.Map<List<Milestone>>(dto.Milestones))
            {
                topic.Milestones.Add(milestone);
            }
            topic.Issues.Clear();
            foreach (var issue in _mapper.Map<List<Issue>>(dto.Issues))
            {
                topic.Issues.Add(issue);
            }
            topic.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Đã cập nhật tiến độ thành công." });
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedTopics()
        {
            var topics = await _context.ResearchTopics
                .Where(t => t.Status == "Approved")
                .ToListAsync();

            var result = _mapper.Map<List<ResearchProgressDto>>(topics);
            return Ok(result);
        }
        [HttpPut("{userId}/{id}/approve")]
        public async Task<IActionResult> ApproveTopic(int id, int userId, [FromBody] ApprovalDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                Console.WriteLine($"userId: {userId}");
                Console.WriteLine($"user: {user.Role}");
                if (user?.Role != "council" && user?.isSeniorCouncilMember == true)
                    return StatusCode(403, new { Message = "Chỉ Senior Council Member mới được phê duyệt." });

                Console.WriteLine($"id topic: {id}");
                var topic = await _context.ResearchTopics
                    .Include(t => t.TopicReviewAssignments)
                    .FirstOrDefaultAsync(t => t.TopicId == id);
                if (topic == null)
                    return NotFound("Không tìm thấy đề tài.");

                var isReviewComplete = topic.TopicReviewAssignments.All(a => a.HasReviewed);
                if (!isReviewComplete)
                    return BadRequest("Đề tài chưa được xét duyệt đầy đủ.");

                topic.Status = dto.Status;
                topic.CouncilFeedback = string.IsNullOrEmpty(topic.CouncilFeedback)
                    ? dto.Feedback
                    : $"{topic.CouncilFeedback} | {dto.Feedback}";
                topic.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(new { Message = $"Đã cập nhật trạng thái: {dto.Status}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error approving topic", Details = ex.Message });
            }
        }
        [HttpPut("{id}/{userId}/review")]
        public async Task<IActionResult> UpdateTopicReview(int id, int userId, [FromBody] ReviewDto dto)
        {
            try
            {
                var assignment = await _context.TopicReviewAssignments
                    .FirstOrDefaultAsync(a => a.TopicId == id && a.ReviewerId == userId);
                if (assignment == null)
                    return Forbid("Bạn không được phân công xét duyệt đề tài này.");
                if (assignment.HasReviewed)
                    return BadRequest("Bạn đã xét duyệt đề tài này rồi.");

                var topic = await _context.ResearchTopics.FindAsync(id);
                if (topic == null)
                    return NotFound("Không tìm thấy đề tài.");

                var review = new Review
                {
                    TopicId = id,
                    FeasibilityScore = dto.Evaluation.Feasibility,
                    NoveltyScore = dto.Evaluation.Novelty,
                    ApplicabilityScore = dto.Evaluation.Applicability,
                    TotalScore = dto.Evaluation.Total,
                    Comments = dto.Feedback,
                    CreatedAt = DateTime.Now,
                    CouncilMemberId = userId
                };
                _context.Reviews.Add(review);

                assignment.HasReviewed = true;
                topic.CouncilFeedback = string.IsNullOrEmpty(topic.CouncilFeedback)
                    ? dto.Feedback
                    : $"{topic.CouncilFeedback} | {dto.Feedback}";
                topic.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                var totalReviewers = await _context.TopicReviewAssignments.CountAsync(a => a.TopicId == id);
                var reviewCount = await _context.TopicReviewAssignments.CountAsync(a => a.TopicId == id && a.HasReviewed);

                return Ok(new
                {
                    ReviewCount = reviewCount,
                    TotalReviewers = totalReviewers,
                    IsReviewComplete = reviewCount == totalReviewers // True khi 3/3 hoàn thành
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating review", Details = ex.Message });
            }
        }
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingTopics()
        {
            var topics = await _researchTopicRepository.GetPendingTopics();
            return Ok(topics);
        }

        [HttpGet("{topicId}/milestones")]
        public async Task<IActionResult> GetMilestonesByTopicId(int topicId)
        {
            var milestones = await _researchTopicRepository.GetMilestonesByTopicIdAsync(topicId);
            return Ok(milestones);
        }
        [HttpPost("reportprogress")]
        public async Task<IActionResult> ReportProgress([FromForm] progress_report_dto progressReportDto, IFormFile file)
        {
            if (progressReportDto == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Đường dẫn lưu file (có thể thay đổi theo cấu hình của bạn)
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

                    // Gán đường dẫn file vào DTO
                    progressReportDto.FilePath = filePath;
                }

                var result = await _researchTopicRepository.reportProgress(progressReportDto);
                if (result)
                {
                    return Ok(new { message = "Progress reported successfully.", filePath = progressReportDto.FilePath });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to report progress.", error = ex.Message });
            }

            return BadRequest(new { message = "Failed to report progress." });
        }

        [HttpGet("approvedbutnotcompleted/{lectureId}")]
        public async Task<IActionResult> GetApprovedButNotCompletedRecords(int lectureId)
        {
            var researchTopics = await _researchTopicRepository.getApprovedButNotCompletedRecords(lectureId);
            if (researchTopics == null || !researchTopics.Any())
            {
                return NotFound("No approved but not completed records found.");
            }
            // Return the list of research topics
            return Ok(researchTopics);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateResearchTopic([FromBody] research_topic_dto researchTopicDto)
        {
            try
            {
                var result = await _researchTopicRepository.CreateResearchTopic(researchTopicDto);
                if (result)
                {
                    return Ok(new { message = "Research topic created successfully." });
                }
                return BadRequest(new { message = "Failed to create research topic." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("researchtopics/{id}")]
        public async Task<IActionResult> DeleteResearchTopic(int id)
        {
            var result = await _researchTopicRepository.DeleteResearchTopic(id);
            if (result)
            {
                return NoContent(); // Successfully deleted
            }
            return NotFound(); // Topic not found
        }
        [HttpGet("researchtopics")]
        public async Task<IActionResult> GetAllResearchTopics()
        {
            var researchTopics = await _researchTopicRepository.GetAllResearchTopics();
            return Ok(researchTopics);
        }

        [HttpGet("researchtopics/{id}")]
        public async Task<IActionResult> GetResearchTopicById(int id)
        {
            var researchTopic = await _researchTopicRepository.GetResearchTopicById(id);
            if (researchTopic == null)
            {
                return NotFound(); // Topic not found
            }
            return Ok(researchTopic);
        }


        [HttpGet("researchtopics/user/{userId}")]
        public async Task<IActionResult> GetResearchTopicsByUserId(int userId)
        {
            var researchTopics = await _researchTopicRepository.GetResearchTopicsByUserId(userId);
            return Ok(researchTopics);
        }

        [HttpPut("researchtopics/{id}")]
        public async Task<IActionResult> UpdateResearchTopic(int id, [FromBody] research_topic_dto researchTopicDto)
        {
            if (researchTopicDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _researchTopicRepository.UpdateResearchTopic(id, researchTopicDto);
            if (result)
            {
                return NoContent(); // Successfully updated
            }
            return NotFound(); // Topic not found
        }


    }
}