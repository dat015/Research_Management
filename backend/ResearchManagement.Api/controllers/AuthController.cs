using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.interfaces;

namespace ResearchManagement.Api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] login_dto loginDto)
        {
            try
            {
                var response = await _authRepository.Login(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _authRepository.Register(registerDto);
                var token = _authRepository.CreateToken(user);
                return Ok(new login_response_dto
                {
                    token = token,
                    user = user,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}