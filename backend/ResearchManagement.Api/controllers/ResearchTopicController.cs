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
    public class ResearchTopicController : ControllerBase
    {
        private readonly IResearchTopicRepository _researchTopicRepository;
        public ResearchTopicController(IResearchTopicRepository researchTopicRepository)
        {
            _researchTopicRepository = researchTopicRepository;
        }
        [HttpPost("reportprogress")]
        public async Task<IActionResult> ReportProgress([FromForm] progress_report_dto progressReportDto, IFormFile file)
        {
            if (progressReportDto == null)
            {
                return BadRequest("Invalid data.");
            }

            // Xử lý file nếu có
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