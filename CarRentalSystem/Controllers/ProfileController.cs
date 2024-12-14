using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Data; // Replace with the namespace containing ApplicationDbContext
using CarRentalSystem.Models; // Replace with the namespace containing UserViewModel
using CarRentalSystem.ViewModels;
using System.Security.Claims;

namespace CarRentalSystem.Controllers
{
    [Authorize] // Ensure only authenticated users can access this controller
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profile/MyAccount
        public async Task<IActionResult> MyAccount()
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not authenticated
            }

            // Fetch the user's details from the database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Map the user data to a UserViewModel
            var model = new UserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
              
            };

            return View(model); // Pass the model to the view
        }

        // POST: Profile/UpdateAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid form submission. Please correct the errors.";
                return View("MyAccount", model); // Return the view with validation errors
            }

            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the user from the database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user details
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
        

            // Save changes to the database
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Account updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return View("MyAccount", model);
            }

            return RedirectToAction("MyAccount");
        }

        // POST: Profile/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid form submission. Please correct the errors.";
                return RedirectToAction("MyAccount");
            }

            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the user from the database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Verify the current password (add logic if needed)
            if (model.CurrentPassword != user.PasswordHash) // Replace with proper password validation logic
            {
                TempData["ErrorMessage"] = "Current password is incorrect.";
                return RedirectToAction("MyAccount");
            }

            // Update the password
            user.PasswordHash = model.NewPassword; // Replace with proper password hashing
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Password updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("MyAccount");
            }

            return RedirectToAction("MyAccount");
        }
    }
}
