using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.Data;
using CarRentalSystem.Models;

namespace CarRentalSystem.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars/Index
        public async Task<IActionResult> Index(string searchQuery, int page = 1, int pageSize = 10)
        {
            IQueryable<Car> carsQuery = _context.Cars;

            // Search functionality
            if (!string.IsNullOrEmpty(searchQuery))
            {
                carsQuery = carsQuery.Where(c => c.Brand.Contains(searchQuery) ||
                                                 c.Model.Contains(searchQuery) ||
                                                 c.Year.ToString().Contains(searchQuery));
            }

            // Pagination
            var totalCars = await carsQuery.CountAsync();
            var cars = await carsQuery
                .OrderBy(c => c.Brand) // Optional: Order alphabetically by brand
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ViewData for current state
            ViewData["CurrentSearch"] = searchQuery;
            ViewData["CurrentPage"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)System.Math.Ceiling((double)totalCars / pageSize);

            return View(cars);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, IFormFile? carImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (carImage != null && carImage.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(carImage.FileName);
                        car.ImageUrl = $"images/{uniqueFileName}"; // Save the image path only

                        // Save the image file to the "wwwroot/images" folder
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await carImage.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        // Set default image if no image is provided
                        car.ImageUrl = "images/placeholder.jpg";
                    }

                    // Add the car to the database
                    _context.Add(car);
                    await _context.SaveChangesAsync();

                    // Set a success message
                    TempData["SuccessMessage"] = "Car added successfully.";
                    return RedirectToAction(nameof(Index)); // Redirect to the Cars List page
                }
                catch (Exception ex)
                {
                    // Log the error (optional)
                    Console.WriteLine($"Error adding car: {ex.Message}");
                    TempData["ErrorMessage"] = "An error occurred while adding the car. Please try again.";
                }
            }

            // If model state is invalid or error occurs, reload the form
            TempData["ErrorMessage"] = "Failed to add the car. Please check the form.";
            return View(car);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/ManageCars
        public async Task<IActionResult> ManageCars(string searchQuery, int page = 1, int pageSize = 10)
        {
            IQueryable<Car> carsQuery = _context.Cars;

            // Search functionality
            if (!string.IsNullOrEmpty(searchQuery))
            {
                carsQuery = carsQuery.Where(c => c.Brand.Contains(searchQuery) ||
                                                 c.Model.Contains(searchQuery) ||
                                                 c.Year.ToString().Contains(searchQuery));
            }

            // Pagination
            var totalCars = await carsQuery.CountAsync();
            var cars = await carsQuery
                .OrderBy(c => c.Brand) // Optional: Order alphabetically by brand
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ViewData for current state
            ViewData["CurrentSearch"] = searchQuery;
            ViewData["CurrentPage"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)System.Math.Ceiling((double)totalCars / pageSize);

            return View(cars);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid car ID.";
                return RedirectToAction(nameof(ManageCars));
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found.";
                return RedirectToAction(nameof(ManageCars));
            }

            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.CarId)
            {
                TempData["ErrorMessage"] = "Car ID mismatch.";
                return RedirectToAction(nameof(ManageCars));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Car details updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        TempData["ErrorMessage"] = "Car not found.";
                        return RedirectToAction(nameof(ManageCars));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "A concurrency error occurred.";
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating car: {ex.Message}");
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                }

                return RedirectToAction(nameof(ManageCars));
            }

            TempData["ErrorMessage"] = "Failed to update car details. Please check the form.";
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car); // This returns the Delete.cshtml view
        }
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Car not found.";
            }

            return RedirectToAction(nameof(ManageCars)); // Redirect back to car management
        }




        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
