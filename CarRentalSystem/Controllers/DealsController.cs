using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Data; // Namespace for ApplicationDbContext
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class DealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch deals from the database and map them to DealViewModel
            var deals = _context.Cars.Select(car => new DealViewModel
            {
                CarId = car.CarId,          // Fetch the car's unique ID
                Brand = car.Brand,          // Fetch the car's brand
                Model = car.Model,          // Fetch the car's model
                ImageUrl = car.ImageUrl,    // Fetch the car's image URL
                PricePerDay = car.PricePerDay // Fetch the price per day
            }).ToList();

            // Pass the deals to the Razor page
            return View(deals);
        }


    }
}
