using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class ResearchTopic //Đề tài nghiên cứu
    {
        [Key]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Mã giảng viên là bắt buộc")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên đề tài là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên đề tài không được vượt quá 255 ký tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Lĩnh vực là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Lĩnh vực không được vượt quá 100 ký tự")]
        public string Field { get; set; }

        [Required(ErrorMessage = "Mục tiêu là bắt buộc")]
        public string Objective { get; set; }

        [Required(ErrorMessage = "Phương pháp là bắt buộc")]
        public string Method { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Kinh phí là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Kinh phí phải là số không âm")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, InProgress, Completed

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; }
        public ICollection<ProgressReport> ProgressReports { get; set; }
        public FinalReport? FinalReport { get; set; }
        public ICollection<Publication> Publications { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Budget? BudgetDetail { get; set; }
        public ICollection<ComplianceRecord> ComplianceRecords { get; set; }
    }
}