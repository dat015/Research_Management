using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class Issue //vấn đề phát sinh
    {
        [Key] // Đánh dấu khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
        public int IssueId { get; set; } // Khóa chính
        public int TopicId { get; set; } // Khóa ngoại liên kết với ResearchTopic
        public string Description { get; set; } // Mô tả vấn đề
        public string Impact { get; set; } // Mức độ ảnh hưởng (low, medium, high)
        public string Status { get; set; } // Trạng thái (open, resolved)
        public string Resolution { get; set; } // Giải pháp (nếu đã giải quyết)
        public int milestone_id { get; set; } // Khóa ngoại liên kết với milestone
      
        // Quan hệ ngược với ResearchTopic
        [ForeignKey("TopicId")]
        public ResearchTopic Topic { get; set; }
        // Quan hệ ngược với milestone
        [ForeignKey("milestone_id")]
        public Milestone Milestone { get; set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime? ResolvedAt { get; internal set; }
    }
}