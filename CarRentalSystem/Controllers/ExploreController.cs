using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Controllers
{
    public class ExploreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BecomeHost()
        {
            return View();
        }

        public IActionResult HowItWorks()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

    }
}
