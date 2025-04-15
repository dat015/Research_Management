using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class ProgressReportIssue //các vấn đề trong kỳ báo cáo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Mô tả vấn đề là bắt buộc")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Tác động là bắt buộc")]
        public string Impact { get; set; }
        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public string Status { get; set; } // "open", "in_progress", "resolved"        public string Resolution { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? Resolution { get; set; }
        public ProgressReport ProgressReport { get; set; }
    }
}