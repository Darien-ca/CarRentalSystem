using System.Collections.Generic;

namespace CarRentalSystem.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<FeaturedCarViewModel> FeaturedCars { get; set; } = new List<FeaturedCarViewModel>();
    }
}
