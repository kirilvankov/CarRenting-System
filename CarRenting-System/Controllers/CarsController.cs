namespace CarRenting_System.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using CarRenting_System.Data;
    using CarRenting_System.Data.Models;
    using CarRenting_System.Models.Cars;

    using Microsoft.AspNetCore.Mvc;
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
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

            var brands = this.data
                        .Cars
                        .Select(c => c.Make)
                        .Distinct()
                        .OrderBy(br => br)
                        .ToList();

            carsQuery = query.Sorting switch
            {
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.MakeAndModel => carsQuery.OrderBy(c => c.Make).ThenBy(c => c.Model),
                CarSorting.DateCreated or _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = carsQuery
                .Skip((query.CurrentPage - 1) * AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
                .Select(c => new CarListingViewModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.ToString()

                }).ToList();

            query.Cars = cars;
            query.TotalCars = totalCars;
            query.Makes = brands;

            return View(query);
        }

        public IActionResult Add() => View(new AddCarFormModel
        {
            Categories = this.GetCategories()
        });

        [HttpPost]
        public IActionResult Add(AddCarFormModel car)
        {
            if (!this.data.Categories.Any(c => c.Id == car.CategoryId))
            {
                ModelState.AddModelError("Category", "Invalid category");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = this.GetCategories();
                return View(car);
            }

            var carData = new Car
            {
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId

            };

            this.data.Cars.Add(carData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }


        private IEnumerable<CarCategoryViewModel> GetCategories()
            => this.data
                    .Categories
                    .Select(c => new CarCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

    }
}
