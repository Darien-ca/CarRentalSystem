namespace CarRentalSystem.ViewModels
{
    public class FeaturedCarViewModel
    {
        public int CarId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string? ImageUrl { get; set; }
    }
}
