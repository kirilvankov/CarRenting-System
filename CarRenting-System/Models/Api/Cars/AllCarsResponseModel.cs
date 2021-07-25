namespace CarRenting_System.Models.Api.Cars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AllCarsResponseModel
    {
        public int CurrentPage { get; init; }

        public int TotalCars { get; set; }

        public IEnumerable<CarApiResponseModel> Cars { get; init; }
    }
}
