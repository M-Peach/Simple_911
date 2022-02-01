using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Simple_911.Data;
using Simple_911.Models;
using Simple_911.Services.Interfaces;

namespace Simple_911.Services
{
    public class RolesService : ITRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<SimpleUser> _userManager;

        public RolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<SimpleUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(SimpleUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(SimpleUser user)
        {
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<bool> RemoveUserFromRolesAsync(SimpleUser user, IEnumerable<string> roles)
        {
            bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
            return result;
        }

        public async Task<List<IdentityRole>> GetRolesAync()
        {
            try
            {
                List<IdentityRole> result = new();


                result = await _context.Roles.ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
