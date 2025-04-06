using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class Budget //Ngân sách
    {
        [Key]
        public int BudgetId { get; set; }

        [Required(ErrorMessage = "Mã đề tài là bắt buộc")]
        [ForeignKey("ResearchTopic")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Số tiền được cấp là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền được cấp phải là số không âm")]
        public decimal AllocatedAmount { get; set; }

        [Required(ErrorMessage = "Số tiền đã sử dụng là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền đã sử dụng phải là số không âm")]
        public decimal UsedAmount { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ResearchTopic ResearchTopic { get; set; }
    }
}