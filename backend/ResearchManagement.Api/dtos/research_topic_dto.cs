using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class research_topic_dto
    {
        public int UserId { get; set; }


        public string Title { get; set; }


        public string Field { get; set; }

        public string Objective { get; set; }

        public string Method { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public decimal Budget { get; set; }


        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, InProgress, Completed


    }
}