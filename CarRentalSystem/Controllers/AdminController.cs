using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.Models;
using CarRentalSystem.Data;

namespace CarRentalSystem.Controllers
{
    [Authorize(Roles = "Admin")] // Restrict all actions in this controller to Admin users
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context; // Add ApplicationDbContext

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context) // Inject ApplicationDbContext
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context; // Assign context to the private field
        }

        public IActionResult Index()
        {
            // If the view expects List<string>, pass the correct model
            var model = new List<string> { "Item1", "Item2", "Item3" };
            return View(model);
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _userManager.Users.Count();
            ViewBag.TotalRoles = _roleManager.Roles.Count();

            return View();
        }

        // GET: Admin/ManageCars
        public IActionResult ManageCars()
        {
            var cars = _context.Cars.ToList(); // Fetch cars from the database
            return View(cars); // Ensure you have a corresponding ManageCars view
        }

        // GET: Admin/ManageUsers
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, List<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.ToList();
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        // GET: Admin/EditUser/{id}
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvailableRoles = roles.Select(r => r.Name).ToList(),
                AssignedRoles = userRoles.ToList()
            };

            return View(model);
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input.";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(ManageUsers));
            }

            // Update user details
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // Update user roles
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.AssignedRoles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(model.AssignedRoles).ToList();

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            TempData["SuccessMessage"] = "User details updated successfully.";
            return RedirectToAction(nameof(ManageUsers));
        }

        // POST: Admin/DeleteUser/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                if (deleteResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "User deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete user.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }
            return RedirectToAction(nameof(ManageUsers));
        }

        // GET: Admin/CreateRole
        public IActionResult CreateRole()
        {
            return View();
        }

        // POST: Admin/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "Role already exists.");
                return View();
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{roleName}' created successfully.";
                return RedirectToAction(nameof(ManageUsers));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }
    }
}
