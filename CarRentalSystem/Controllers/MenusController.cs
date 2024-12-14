using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Controllers
{
    public class MenusController : Controller
    {
        public IActionResult TopMenu()
        {
            var menuItems = new List<string>
    {"Home","Cars","Add Car","Rentals", "About Us", "Contact Us"};

            if (User.Identity != null && User.IsInRole("Admin"))
            {
                menuItems.Add("Dashboard");
            }

            return PartialView("_TopMenu", menuItems.Distinct().ToList()); // Remove duplicates with Distinct()
        }

    }
}
