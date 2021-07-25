namespace CarRenting_System.Controllers.Api
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using CarRenting_System.Data;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Api.Cars;
    using CarRenting_System.Services.Cars;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly ICarsService cars;

        public CarsApiController(ICarsService cars)
        {
            this.cars = cars;
        }

        [HttpGet]
        public AllCarsApiServiceModel All([FromQuery] AllCarsApiRequestModel query) 
            => this.cars.AllCars(
                    query.Make,
                    query.SearchTerm,
                    query.CategoryId,
                    query.Sorting,
                    query.CurrentPage,
                    query.CarsPerPage);

    }
}
