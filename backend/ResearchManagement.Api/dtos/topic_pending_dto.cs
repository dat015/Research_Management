using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class topic_pending_dto
    {
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Lecturer { get; set; }
        public string Field { get; set; }
        public string Status { get; set; }
        public string CouncilFeedback { get; set; }
        public int ReviewCount { get; set; }
        public int TotalReviewers { get; set; }
        public bool IsReviewComplete { get; set; }
        public List<ReviewerAssignmentDto> AssignedReviewers { get; set; }
        public List<ReviewerDto> RemainingReviewers { get; set; }
        public DateTime? ReviewDate { get; set; } // Ngày xét duyệt cuối cùng
        public EvaluationDto Evaluation { get; set; } // Điểm trung bình
        public DateTime? RevisionDeadline { get; set; } // Hạn chỉnh sửa
    }
    public class ReviewerAssignmentDto
    {
        public int ReviewerId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public bool HasReviewed { get; set; }
        public EvaluationDto Evaluation { get; set; } // Thêm đánh giá chi tiết
        public string Feedback { get; set; } // Thêm feedback riêng
        public DateTime? ReviewedAt { get; set; } // Thời gian xét duyệt
    }

    public class ReviewerDto
    {
        public int ReviewerId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
    }
}