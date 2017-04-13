using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FootballCoachOnline.Models;
using FootballCoachOnline.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballCoachOnline.Controllers
{
    [Authorize(Roles="Administrator")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string description)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole { Name = name, Description = description };
                await roleManager.CreateAsync(role);

                return RedirectToAction("Index");
            }

            return View(name, description);
        }

        public IActionResult Join()
        {
            UserRoleViewModel model = new UserRoleViewModel
            {
                RoleList = new SelectList(roleManager.Roles, "Id", "Name"),
                UserList = new SelectList(userManager.Users, "Id", "UserName")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(string UserId, string RoleId)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(UserId);
                var role = await roleManager.FindByIdAsync(RoleId);
                await userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction("Details", new { roleName = role.Name});
            }

            return View();
        }

        public async Task<IActionResult> Details(string roleName)
        {
            if(String.IsNullOrEmpty(roleName))
            {
                return NotFound();
            }

            var users = await userManager.GetUsersInRoleAsync(roleName);
            ViewBag.Role = roleName;

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string role, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveFromRoleAsync(user, role);

            return RedirectToAction("Details", new { roleName = role });
        }
    }
}