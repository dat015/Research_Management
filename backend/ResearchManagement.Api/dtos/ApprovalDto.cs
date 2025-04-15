using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class ApprovalDto
    {
        public string Status { get; set; } // "Approved", "Rejected", "NeedsRevision"
        public string Feedback { get; set; } // Phản hồi từ Senior Council Member
    }
}