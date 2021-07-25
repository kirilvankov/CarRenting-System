namespace CarRenting_System.Services.Cars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AllCarsApiServiceModel
    {
        public int CurrentPage { get; init; }

        public int TotalCars { get; init; }

        public int CarsPerPage { get; init; }

        public IEnumerable<CarServiceModel> Cars { get; init; }
    }
}
