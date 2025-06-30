using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactoryEntitlementProgram.Services
{
    public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<AppUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<AppUser> GetUserByNameAsync(string userName)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IList<string>> GetUserRolesAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(AppUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task AddUserAsync(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new System.Exception(string.Join(", ", result.Errors));
            }
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new System.Exception(string.Join(", ", result.Errors));
            }
        }

        public async Task DeleteUserAsync(AppUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new System.Exception(string.Join(", ", result.Errors));
            }
        }
    }
}
