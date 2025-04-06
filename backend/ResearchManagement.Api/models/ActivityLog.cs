using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class ActivityLog //Nhật ký hoạt động
    {
        [Key]
        public int LogId { get; set; }

        [Required(ErrorMessage = "Mã người dùng là bắt buộc")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Hành động là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Hành động không được vượt quá 255 ký tự")]
        public string Action { get; set; }

        public string? Details { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public User User { get; set; }
    }
}