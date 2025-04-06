using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class ComplianceRecord //Tuân thủ quy định (Đảm bảo đề tài tuân thủ quy định đạo đức và trường học.)
    {
        [Key]
        public int ComplianceId { get; set; }

        [Required(ErrorMessage = "Mã đề tài là bắt buộc")]
        [ForeignKey("ResearchTopic")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Quy định là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Quy định không được vượt quá 255 ký tự")]
        public string Rule { get; set; }

        [Required(ErrorMessage = "Trạng thái tuân thủ là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string Status { get; set; } // Compliant, NonCompliant

        public string? Comments { get; set; }

        [Required]
        public DateTime CheckedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ResearchTopic ResearchTopic { get; set; }
    }
}