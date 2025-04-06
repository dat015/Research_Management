using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class FinalReport//Báo cáo kết quả
    {
        [Key]
        public int FinalReportId { get; set; }

        [Required(ErrorMessage = "Mã đề tài là bắt buộc")]
        [ForeignKey("ResearchTopic")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Tóm tắt báo cáo là bắt buộc")]
        public string Summary { get; set; }

        [MaxLength(255, ErrorMessage = "Đường dẫn tệp không được vượt quá 255 ký tự")]
        public string? FilePath { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ResearchTopic ResearchTopic { get; set; }
    }
}