namespace CarRentalSystem.ViewModels
{
    public class CarDetailsViewModel
    {
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Features { get; set; }
        public string Availability { get; set; } = string.Empty;

        public int Mileage { get; set; }

        public string FuelType { get; set; } = string.Empty;

        public int Seats { get; set; }
        public string Transmission { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;


    }

}
