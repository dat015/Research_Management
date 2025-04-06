using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.dtos
{
    public class login_response_dto
    {
        public string token { get; set; }
        public User? user { get; set; }
        public bool success { get; set; }
    }
}