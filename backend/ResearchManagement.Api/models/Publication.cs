using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class Publication //Công bố khoa học
    {
        [Key]
        public int PublicationId { get; set; }

        [Required(ErrorMessage = "Mã đề tài là bắt buộc")]
        [ForeignKey("ResearchTopic")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Tên công bố là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên công bố không được vượt quá 255 ký tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Loại công bố là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Loại công bố không được vượt quá 50 ký tự")]
        public string Type { get; set; } // Journal, Conference, Other

        [Required(ErrorMessage = "Năm công bố là bắt buộc")]
        [Range(1900, 9999, ErrorMessage = "Năm công bố không hợp lệ")]
        public int Year { get; set; }

        [MaxLength(255, ErrorMessage = "Đường dẫn tệp không được vượt quá 255 ký tự")]
        public string? FilePath { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ResearchTopic ResearchTopic { get; set; }
    }
}