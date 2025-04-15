using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class ProgressReport//Báo cáo tiến độ
    {
        [Key]
        public int ReportId { get; set; }

        [Required(ErrorMessage = "Mã đề tài là bắt buộc")]
        [ForeignKey("ResearchTopic")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Ngày báo cáo là bắt buộc")]
        public DateTime ReportDate { get; set; }

        [Required(ErrorMessage = "Mô tả tiến độ là bắt buộc")]
        public string Description { get; set; }

        [MaxLength(255, ErrorMessage = "Đường dẫn tệp không được vượt quá 255 ký tự")]
        public string? FilePath { get; set; }
        public decimal? usedAmount { get; set; } // Số tiền đã sử dụng trong báo cáo tiến độ

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Mã mốc tiến độ là bắt buộc")]
        [ForeignKey("Milestone")]
        public int MilestoneId { get; set; } // Liên kết với mốc tiến độ

        // Navigation property
        public ResearchTopic ResearchTopic { get; set; }
        public Milestone Milestone { get; set; }
        public List<ProgressReportIssue> Issues { get; set; } = new List<ProgressReportIssue>(); // Danh sách các vấn đề liên quan
    }
}