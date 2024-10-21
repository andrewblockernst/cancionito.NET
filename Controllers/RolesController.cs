using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

    public RolesController(IRoleService roleService, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
    {
        _roleService = roleService;
        _userManager = userManager;
    }

    // Crear un nuevo rol
    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest(new { Message = "El nombre del rol no puede estar vacío" });
        }

        var result = await _roleService.CreateRoleAsync(roleName);
        if (result)
        {
            return Ok(new { Message = "Rol creado correctamente" });
        }
        return BadRequest(new { Message = "No se pudo crear el rol" });
    }

    // Asignar un rol a un usuario
    [HttpPost("assign-rol")]
    public async Task<IActionResult> AsignarRol([FromBody] RoleAssignmentDTO model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Role))
        {
            return BadRequest(new { Message = "Datos de entrada no válidos" });
        }

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return NotFound(new { Message = "Usuario no encontrado" });
        }

        var roleExists = await _userManager.IsInRoleAsync(user, model.Role);
        if (roleExists)
        {
            return BadRequest(new { Message = "El usuario ya tiene este rol" });
        }

        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (result.Succeeded)
        {
            return Ok(new { Message = "Rol asignado correctamente" });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error al asignar el rol" });
    }
}
