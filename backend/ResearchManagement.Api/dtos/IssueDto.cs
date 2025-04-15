using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class IssueDto
    {
        public int? IssueId { get; set; } // ID của vấn đề
        public int TopicId { get; set; } // ID của đề tài nghiên cứu
        public string Description { get; set; } // Mô tả vấn đề
        public string Impact { get; set; } // Mức độ ảnh hưởng (low, medium, high)
        public string Status { get; set; } // Trạng thái (open, resolved)
        public string Resolution { get; set; } // Giải pháp
    }
}