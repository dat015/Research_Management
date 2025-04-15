using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class TopicReviewAssignment
    {
        //Bước 1: Khi một đề tài được nộp (Status = "Pending"), hệ thống kiểm tra Field của đề tài.
        //Bước 2: Lấy danh sách Council Members có Department khớp với Field.
        // Bước 3: Chọn ngẫu nhiên 3 người (hoặc số lượng cấu hình) từ danh sách khớp.
        // Nếu không đủ 3 người trong department, chọn thêm từ các department khác, ưu tiên người có ít đề tài đang xét duyệt nhất.
        // Bước 4: Lưu phân công vào TopicReviewAssignments.
        [Key]
        public int AssignmentId { get; set; }
        public int? TopicId { get; set; }
        public int? ReviewerId { get; set; }
        public bool HasReviewed { get; set; }

        [ForeignKey("TopicId")]
        public ResearchTopic? ResearchTopic { get; set; }
        [ForeignKey("ReviewerId")]
        public User? Reviewer { get; set; }
    }
}