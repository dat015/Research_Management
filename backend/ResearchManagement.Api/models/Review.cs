using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class Review //Xét duyệt đề tài
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Mã đề tài là bắt buộc")]
        [ForeignKey("ResearchTopic")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Mã thành viên hội đồng là bắt buộc")]
        [ForeignKey("User")]
        public int CouncilMemberId { get; set; }

        [Range(1, 10, ErrorMessage = "Điểm tính khả thi phải từ 1 đến 10")]
        public int? FeasibilityScore { get; set; }

        [Range(1, 10, ErrorMessage = "Điểm tính mới phải từ 1 đến 10")]
        public int? NoveltyScore { get; set; }

        [Range(1, 10, ErrorMessage = "Điểm tính ứng dụng phải từ 1 đến 10")]
        public int? ApplicabilityScore { get; set; }

        public string? Comments { get; set; }

        [MaxLength(50, ErrorMessage = "Quyết định không được vượt quá 50 ký tự")]
        public string? Decision { get; set; } // Approved, Rejected, NeedsRevision

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ResearchTopic ResearchTopic { get; set; }
        public User User { get; set; }
    }
}