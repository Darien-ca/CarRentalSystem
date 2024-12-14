using System.Collections.Generic;

namespace CarRentalSystem.ViewModels
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> AvailableRoles { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
