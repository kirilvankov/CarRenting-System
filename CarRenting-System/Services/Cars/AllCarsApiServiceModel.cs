namespace CarRenting_System.Services.Cars
{
 
    using System.Collections.Generic;

    public class AllCarsApiServiceModel
    {
        public int CurrentPage { get; init; }

        public int TotalCars { get; init; }

        public int CarsPerPage { get; init; }

        public IEnumerable<CarServiceModel> Cars { get; init; }

    }
}
