using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class ReviewDto
    {
        public EvaluationDto Evaluation { get; set; }
        public string Feedback { get; set; }
    }
}