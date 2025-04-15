using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class budget_dto
    {
        public int TopicId { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}