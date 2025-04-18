using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class ResearchProgressDto
    {
        public int Id { get; set; } // Mã đề tài (TopicId dưới dạng string)
        public string Title { get; set; } // Tên đề tài
        public string Lecturer { get; set; } // Giảng viên
        public string Department { get; set; } // Khoa/Phòng ban
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime EndDate { get; set; } // Ngày kết thúc
        public int CurrentProgress { get; set; } // Tiến độ hiện tại
        public string Status { get; set; } // Trạng thái (on_track, delayed, completed) - Tính toán ở frontend
        public List<MilestoneDto> Milestones { get; set; } // Danh sách mốc tiến độ
        public List<IssueDto> Issues { get; set; } // Danh sách vấn đề
    }
}