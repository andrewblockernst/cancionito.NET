using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    //CONSTRUCTOR THAT INYECTS THE USERMANAGER, ROLEMANAGER, SIGNINMANAGER AND ALL THE MANAGER OF THE USERS
    public AccountController(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        SignInManager<ApplicationUser> signInManager, 
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    //REGISTRATION OF A NEW USER
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        //VERIFICATION TO CHECK IF THE USER ALREADY EXISTS
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The user already exist." });
        }

        //NEW USER CREATION
        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        //CREATION OF THE USER
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error creating an user." });
        }

        return Ok(new { Message = "User succesfully created." });
    }

    //LOGIN OF A USER WITH USERNAME AND PASSWORD (GENERATING A TOKEN) 
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model) {

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) {
            var userRoles = await _userManager.GetRolesAsync(user);

            //IT DEFINES THE CLAIMS OF THE USER, INCLUDING THE NAME AND THE UNIQUE IDENTIFIER OF THE TOKEN (ROLE)
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            };

            //ADD THE ROLES FOR THE USER AS CLAIMS
            foreach (var role in userRoles) {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            //IT DEFINES THE KEY OF THE JWT
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey)) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "JWT Key is not configured." });
            }

            //IT DEFINES THE SIGNING KEY OF THE JWT
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            //IT DEFINES THE EXPIRATION OF THE TOKEN, IF THE USER HAS THE ROLE "BOT" IT WILL NOT EXPIRE
            var tokenExpiry = userRoles.Any(role => role.Equals("Bot", StringComparison.OrdinalIgnoreCase))
                ? DateTime.Now.AddYears(100) //LONG LIVE TO CancioNito (100 YEARS)
                : DateTime.Now.AddHours(3); //NORMAL EXPIRATION OF THE TOKEN (3 HOURS - REST OF THE ROLES)

            //CREATE AND DEFINES THE TOKEN WITH THE CLAIMS, THE EXPIRATION AND THE SIGNING CREDENTIALS
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: tokenExpiry, //EXPIRATION OF THE TOKEN, EXCEPTING THE ROLE "BOT"
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    //ASSIGNATION OF A ROLE TO A USER
    [HttpPost("assign-role")]
    public async Task<IActionResult> AsignarRol([FromBody] RoleAssignmentDTO model) {

        //VERIFICATION OF THE USER
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) {
            return NotFound(new { Message = "User not found." });
        }

        //VERIFICATION OF THE ROLE
        var roleExists = await _userManager.IsInRoleAsync(user, model.Role);
        if (roleExists) {
            return BadRequest(new { Message = "The user already has this role." });
        }

        //ASSIGNATION OF THE ROLE
        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (result.Succeeded) {
            return Ok(new { Message = "Role assigned correctly." });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error assigning role." });
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles() {

        //GET ALL THE ROLES
        List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
        return Ok(roles);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers() {

        //GET ALL THE USERS
        List<ApplicationUser> users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("/users/{id}/roles")]
    public async Task<IActionResult> GetRoles(string id) {

        //GET THE ROLES OF A USER
        var user = await _userManager.FindByIdAsync(id);
        if (user != null) {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        return BadRequest();
    }

    [HttpPost("role")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName) {

        //VERIFICATION OF THE ROLE NAME
        if (string.IsNullOrWhiteSpace(roleName)) {
            return BadRequest("The role name cannot be empty.");
        }

        //VERIFICATION OF THE EXISTENCE OF THE ROLE
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists) {
            return Conflict($"The role '{roleName}' already exists.");
        }

        //CREATION OF THE ROLE
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (result.Succeeded) {
            return Ok($"The role '{roleName}' already exists.");
        }

        //IN CASE OF AN ERROR IN THE CREATION OF THE ROLE - INTERNAL SERVER ERROR (500)
        return BadRequest(result.Errors);
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(string id) {

        //DELETE A USER
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) {
            return NotFound(new { Message = "User not found." });
        }

        //DELETE THE USER
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded) {
            return Ok(new { Message = "Successfully deleted user." });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error deleting user." });
    }

}