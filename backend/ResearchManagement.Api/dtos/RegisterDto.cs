using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchManagement.Api.dtos
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Lecturer, CouncilMember, Admin
        public string? Department { get; set; }
    }
}