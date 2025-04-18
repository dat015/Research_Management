using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class FinalReportDto
    {
        public int FinalReportId { get; set; } // Mã báo cáo
        public int TopicId { get; set; } // Mã đề tài
        public string Summary { get; set; } // Tóm tắt báo cáo
        public string? FilePath { get; set; } // Đường dẫn tệp báo cáo
        public DateTime CreatedAt { get; set; } // Ngày tạo báo cáo
        public string TopicTitle { get; set; } // Tên đề tài
        public string Lecturer { get; set; } // Giảng viên phụ trách
        public string Department { get; set; } // Khoa/Phòng ban
        public string email {get; set;}
        
    }
}