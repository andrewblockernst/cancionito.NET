using Microsoft.AspNetCore.Identity;

public class RoleService : IRoleService {
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public async Task<bool> CreateRoleAsync(string roleName) {
        if (!await _roleManager.RoleExistsAsync(roleName)) {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }
        return false;
    }
    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName) {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !await _roleManager.RoleExistsAsync(roleName)) {
            return false;
        }
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }
}
