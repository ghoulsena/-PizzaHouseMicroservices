using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOS;
using UserService.Application.Services;
using UserService.Domain.Entities;

namespace UserService.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        private readonly UserAppService _userService;

        private readonly ILogger<AuthController> _logger;

        public AuthController(UserAppService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequestDTO loginRequest)
        {
            
            var token = await _userService.Login(loginRequest.Email, loginRequest.Password);
            if (token == null)
            {
                _logger.LogWarning("Неудачная попытка входа для {Email}", loginRequest.Email);
                return Unauthorized("Неверный логин или пароль");
            }
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var user = await _userService.RegisterAsync(
                dto.UserName,
                dto.Email,
                dto.Password
            );

            return Ok(new
            {
                user.Id,
                user.UserName,
                Email = user.Email.Value
            });
        }


    }

}

