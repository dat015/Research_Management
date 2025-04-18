using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class email_dto
    {
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public string Congratulations { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string ThankYouMessage { get; set; }
    }
    public class RejectionDto
    {
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public string RejectionReason { get; set; }
    }
}