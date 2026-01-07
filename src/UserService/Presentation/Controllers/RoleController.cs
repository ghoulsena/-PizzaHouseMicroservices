using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOS;
using UserService.Application.Services;

namespace UserService.Presentation.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(RoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRole(RoleDto dto)
        {
            try
            {
                var result = await _roleService.AssignRoleToUserAsync(dto.UserId, dto.RoleName);

                if (!result.Success)
                {
                    _logger.LogWarning("Не удалось добавить роль: {Message}", result.Message);
                    return NotFound(result.Message);
                }

                _logger.LogInformation("Роль '{Role}'  назначена пользователю {UserId}", dto.RoleName, dto.UserId);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка  добавления роли {Role} пользователю {UserId}", dto.RoleName, dto.UserId);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
