namespace CarRenting_System.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 2;

        [Display(Name ="Brand")]
        public string Make { get; init; }

        [Display(Name = "Categories")]
        public int CategoryId { get; set; }

        public int CurrentPage { get; init; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }

        public int TotalCars { get; set; }

        [Display(Name ="Brands")]
        public IEnumerable<string> Makes { get; set; }

        public IEnumerable<CarListingViewModel> Cars { get; set; }

        public IEnumerable<CarCategoryViewModel> Categories { get; set; }
    }
}
