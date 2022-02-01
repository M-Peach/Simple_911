using Microsoft.AspNetCore.Identity;
using Simple_911.Models;

namespace Simple_911.Services.Interfaces
{
    public interface ITRolesService
    {
        public Task<IEnumerable<string>> GetUserRolesAsync(SimpleUser user);
        public Task<bool> AddUserToRoleAsync(SimpleUser user, string roleName);
        public Task<bool> RemoveUserFromRolesAsync(SimpleUser user, IEnumerable<string> roles);
        public Task<List<IdentityRole>> GetRolesAync();
    }
}
