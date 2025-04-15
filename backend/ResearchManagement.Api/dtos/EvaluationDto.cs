using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class EvaluationDto
    {
        public int Feasibility { get; set; }
        public int Novelty { get; set; }
        public int Applicability { get; set; }
        public float Total { get; set; }
    }
}