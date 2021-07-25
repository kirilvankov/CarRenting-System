namespace CarRenting_System.Controllers.Api
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using CarRenting_System.Data;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Api.Cars;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;

        public CarsApiController(CarRentingDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        public ActionResult<AllCarsResponseModel> All([FromQuery] AllCarsApiRequestModel query)
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Make))
            {
                carsQuery = carsQuery.Where(c => c.Make == query.Make);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                        (c.Make + " " + c.Model).ToLower().Contains(query.SearchTerm.ToLower()) ||
                         c.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            if (query.CategoryId != 0)
            {
                carsQuery = carsQuery.Where(c => c.CategoryId == query.CategoryId);
            }

                      

            carsQuery = query.Sorting switch
            {
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.MakeAndModel => carsQuery.OrderBy(c => c.Make).ThenBy(c => c.Model),
                CarSorting.DateCreated or _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = carsQuery
                .Skip((query.CurrentPage - 1) * query.CarsPerPage)
                .Take(query.CarsPerPage)
                .Select(c => new CarApiResponseModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name

                }).ToList();

            return new AllCarsResponseModel
            {
                TotalCars = totalCars,
                Cars = cars,
                CurrentPage = query.CurrentPage
            };

        }
    }
}
