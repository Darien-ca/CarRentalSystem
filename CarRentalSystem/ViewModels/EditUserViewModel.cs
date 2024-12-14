using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        public List<string> AvailableRoles { get; set; } = new List<string>();

        public List<string> AssignedRoles { get; set; } = new List<string>();
    }
}
