using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simple_911.Data;
using Simple_911.Models;
using Simple_911.Models.ViewModels;
using Simple_911.Services.Interfaces;

namespace Simple_911.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITRolesService _rolesService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<SimpleUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public AdminController(ITRolesService rolesService, ApplicationDbContext context, UserManager<SimpleUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _rolesService = rolesService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new()
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("CreateRole");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);


        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new();

            List<SimpleUser> users = await _context.Users.ToListAsync();

            foreach (SimpleUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();

                viewModel.User = user;

                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);

                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAync(), "Name", "Name", selected);

                model.Add(viewModel);
            }


            return View(model);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            SimpleUser simpleUser = (await _context.Users.ToListAsync()).FirstOrDefault(u => u.Id == member.User.Id);

            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(simpleUser);

            string userRole = member.SelectedRoles.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                if (await _rolesService.RemoveUserFromRolesAsync(simpleUser, roles))
                {
                    await _rolesService.AddUserToRoleAsync(simpleUser, userRole);
                }

            }

            return RedirectToAction(nameof(ManageUserRoles));
        }
    }
}
