using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required(ErrorMessage = "Mã người dùng là bắt buộc")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("ResearchTopic")]
        public int? TopicId { get; set; }

        [Required(ErrorMessage = "Nội dung thông báo là bắt buộc")]
        public string Message { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; }
        public ResearchTopic? ResearchTopic { get; set; }
    }
}