using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ResearchManagement.Api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] string filePath)
        {
            try
            {
                // Kiểm tra filePath hợp lệ
                if (string.IsNullOrEmpty(filePath))
                {
                    return BadRequest("Đường dẫn tệp không được để trống.");
                }

                // Đảm bảo filePath nằm trong thư mục Uploads để bảo mật
                var fullPath = Path.Combine(_uploadFolder, Path.GetFileName(filePath));
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound("Tệp không tồn tại.");
                }

                // Đọc file dưới dạng stream
                var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                var fileName = Path.GetFileName(fullPath);

                // Trả về file với Content-Type phù hợp
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        private string GetContentType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".zip":
                    return "application/zip";
                case ".rar":
                    return "application/x-rar-compressed";
                default:
                    return "application/octet-stream";
            }
        }
    }

    // Model cho request
    public class FileDownloadRequest
    {
        [Required]
        public string FilePath { get; set; }
    }
}
