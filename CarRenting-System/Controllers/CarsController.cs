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

        public IActionResult Add() => View(new AddCarFormModel 
        {
            Categories = this.GetCategories()
        });

        [HttpPost]
        public IActionResult Add(AddCarFormModel car)
        {
            if (!this.data.Categories.Any(c=>c.Id == car.CategoryId))
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

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success() => View();

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
