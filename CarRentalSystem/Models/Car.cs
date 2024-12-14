namespace CarRentalSystem.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string? ImageUrl { get; set; }

        public bool Availability { get; set; } 

        // New properties
        public string Description { get; set; } = string.Empty; 
        public int Mileage { get; set; } 
        public string FuelType { get; set; } = string.Empty; 
        public string Transmission { get; set; } = string.Empty; 
        public int Seats { get; set; }
        public string Location { get; set; } = string.Empty;


        public string Category { get; set; }
    }
}

