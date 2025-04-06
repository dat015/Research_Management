using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.interfaces
{
    public interface IAuthRepository
    {
        Task<login_response_dto> Login(login_dto loginDto);
        string CreateToken(User user);
        Task<bool> UserExists(string email);
        Task<User> Register(RegisterDto registerDto);
    }
}