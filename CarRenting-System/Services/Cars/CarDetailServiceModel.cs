namespace CarRenting_System.Services.Cars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CarRenting_System.Data.Models;

    public class CarDetailServiceModel : CarServiceModel
    {
        public string Description { get; init; }

        public int CategoryId { get; init; }

        public int DealerId { get; init; }

        public string UserId { get; init; }
    }
}
