namespace CarRenting_System.Models.Api.Cars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AllCarsApiRequestModel
    {
        public string Make { get; init; }
        
        public int CategoryId { get; set; }

        public int CurrentPage { get; init; } = 1;

        public int CarsPerPage { get; init; } = 10;

        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }

        public int TotalCars { get; set; }

    }
}
