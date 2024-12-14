public class DealViewModel
{
    public int CarId { get; set; } // The unique ID of the car
    public string Brand { get; set; } // The car's brand (e.g., Toyota, Ford)
    public string Model { get; set; } // The car's model (e.g., Camry, Escape)
    public string ImageUrl { get; set; } // URL of the car's image
    public decimal PricePerDay { get; set; } // Rental price per day
}
