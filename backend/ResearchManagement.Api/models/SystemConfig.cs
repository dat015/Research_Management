using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.models
{
    public class SystemConfig
    {
        [Key]
        public int ConfigId { get; set; }

        [Required(ErrorMessage = "Tên khóa là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên khóa không được vượt quá 100 ký tự")]
        public string ConfigKey { get; set; }

        [Required(ErrorMessage = "Giá trị cấu hình là bắt buộc")]
        public string ConfigValue { get; set; }

        [MaxLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string? Description { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}