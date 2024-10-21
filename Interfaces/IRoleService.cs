public interface IRoleService {
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> AssignRoleToUserAsync(string userId, string roleName);
}