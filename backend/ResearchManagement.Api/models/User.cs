using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Mật khẩu không được vượt quá 255 ký tự")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Vai trò không được vượt quá 50 ký tự")]
        public string Role { get; set; } // Lecturer, CouncilMember, Admin
        public bool isSeniorCouncilMember { get; set; } = false; // true nếu là hội đồng trưởng   

        [MaxLength(100, ErrorMessage = "Khoa/Phòng ban không được vượt quá 100 ký tự")]
        public string? Department { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<ResearchTopic>? ResearchTopics { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<ActivityLog>? ActivityLogs { get; set; }
    }
}