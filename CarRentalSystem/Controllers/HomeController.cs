using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the Home page with only the top 6 featured cars
        public IActionResult Index()
        {
            // Fetch the top 6 cars based on price per day (or any other criteria)
            var featuredCars = _context.Cars
                .OrderByDescending(c => c.PricePerDay) // Sort by price (highest to lowest)
                .Take(6) // Take only the top 6
                .Select(c => new FeaturedCarViewModel
                {
                    CarId = c.CarId,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    PricePerDay = c.PricePerDay,
                    ImageUrl = c.ImageUrl // Ensure the ImageUrl is correctly set
                })
                .ToList();

            // Pass the mapped FeaturedCarViewModel to the HomeViewModel
            var model = new HomeViewModel
            {
                FeaturedCars = featuredCars
            };

            return View(model);
        }

        // Displays the Car List page with all cars
        public IActionResult CarList()
        {
            // Fetch all cars from the database
            var allCars = _context.Cars
                .Select(c => new FeaturedCarViewModel
                {
                    CarId = c.CarId,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    PricePerDay = c.PricePerDay,
                    ImageUrl = c.ImageUrl
                })
                .ToList();

            // Create a view model to pass all cars to the view
            var model = new HomeViewModel
            {
                FeaturedCars = allCars // You can reuse the FeaturedCarViewModel for simplicity
            };

            return View(model);
        }

        // Displays the "About Us" page
        public IActionResult AboutUs()
        {
            return View();
        }

        // Displays the "Contact Us" page
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
