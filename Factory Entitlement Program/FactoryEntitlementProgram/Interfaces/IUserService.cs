using FactoryEntitlementProgram.Models;
using System.Security.Claims;

namespace FactoryEntitlementProgram.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<AppUser> GetUserByNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(AppUser user);
        Task<bool> IsUserInRoleAsync(AppUser user, string role);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task AddUserAsync(AppUser user, string password);
        Task UpdateUserAsync(AppUser user);
        Task DeleteUserAsync(AppUser user);
        Task<AppUser> FindByUsernameAsync(string username);
        Task<AppUser> GetCurrentUserAsync(ClaimsPrincipal user);

    }
}
