using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ResearchManagement.Api.dtos;

namespace ResearchManagement.Api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptEvaluation([FromBody] email_dto dto)
        {
            _logger.LogInformation("Starting acceptance email process");
            try
            {
                // Format thời gian
                var parsedTime = DateTime.Parse(dto.Time);
                var formattedTime = parsedTime.ToString("HH:mm 'ngày' dd/MM/yyyy");

                // Tạo HTML email với format chuẩn
                string body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                        .container {{ padding: 20px; }}
                        .greeting {{ font-weight: bold; margin-bottom: 15px; }}
                        .content {{ margin-bottom: 15px; }}
                        .signature {{ margin-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='greeting'>
                            Kính gửi {HttpUtility.HtmlEncode(dto.LecturerName)},
                        </div>
                        <div class='content'>
                            <p>{HttpUtility.HtmlEncode(dto.Congratulations)}</p>
                            
                            <p><strong>Thời gian:</strong> {HttpUtility.HtmlEncode(formattedTime)}</p>
                            <p><strong>Địa điểm:</strong> {HttpUtility.HtmlEncode(dto.Location)}</p>
                            
                            {(string.IsNullOrEmpty(dto.ThankYouMessage) ? "" : $"<p>{HttpUtility.HtmlEncode(dto.ThankYouMessage)}</p>")}
                        </div>
                        <div class='signature'>
                            <p>Trân trọng,<br/>Ban Quản lý Đề tài</p>
                        </div>
                    </div>
                </body>
                </html>";

                _logger.LogInformation($"Sending acceptance email to: {dto.LecturerEmail}");
                _logger.LogDebug($"Email content: {body}");

                await _emailService.SendEvaluationEmailAsync(
                    dto.LecturerEmail,
                    "Thông báo chấp nhận nghiệm thu đề tài",
                    body,
                    true
                );

                _logger.LogInformation("Acceptance email sent successfully");
                return Ok(new { message = "Email thông báo đã được gửi thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending acceptance email: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Lỗi khi gửi email: {ex.Message}" });
            }
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectEvaluation([FromBody] RejectionDto dto)
        {
            _logger.LogInformation("Starting rejection email process");
            try
            {
                string body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                        .container {{ padding: 20px; }}
                        .greeting {{ font-weight: bold; margin-bottom: 15px; }}
                        .content {{ margin-bottom: 15px; }}
                        .reason {{ margin: 15px 0; padding: 10px; background-color: #f8f9fa; }}
                        .signature {{ margin-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='greeting'>
                            Kính gửi {HttpUtility.HtmlEncode(dto.LecturerName)},
                        </div>
                        <div class='content'>
                            <p>Chúng tôi rất tiếc phải thông báo rằng đề tài của bạn chưa được chấp nhận nghiệm thu.</p>
                            
                            <div class='reason'>
                                <p><strong>Lý do:</strong></p>
                                <p>{HttpUtility.HtmlEncode(dto.RejectionReason)}</p>
                            </div>
                        </div>
                        <div class='signature'>
                            <p>Trân trọng,<br/>Ban Quản lý Đề tài</p>
                        </div>
                    </div>
                </body>
                </html>";

                _logger.LogInformation($"Sending rejection email to: {dto.LecturerEmail}");

                await _emailService.SendEvaluationEmailAsync(
                    dto.LecturerEmail,
                    "Thông báo từ chối nghiệm thu đề tài",
                    body,
                    true
                );

                _logger.LogInformation("Rejection email sent successfully");
                return Ok(new { message = "Email thông báo đã được gửi thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending rejection email: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi khi gửi email: {ex.Message}" });
            }
        }

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                string testBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; }}
                        .container {{ padding: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Test Email</h2>
                        <p>This is a test email from the Research Management System.</p>
                        <p>Time sent: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}</p>
                    </div>
                </body>
                </html>";

                await _emailService.SendEvaluationEmailAsync(
                    "6351071016@st.utc2.edu.vn",
                    "Test Email from Research Management System",
                    testBody,
                    true
                );

                return Ok(new { message = "Test email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error sending test email",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}