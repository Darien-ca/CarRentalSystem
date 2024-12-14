using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? EmailID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; } // Optional, used for "Remember Me" functionality
    }
}
