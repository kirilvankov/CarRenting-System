﻿namespace CarRenting_System.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    
    using CarRenting_System.Data;
    using CarRenting_System.Data.Infrastucture;
    using CarRenting_System.Data.Models;
    using CarRenting_System.Models.Cars;
    using CarRenting_System.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IDealerService dealerService;

        public CarsController(CarRentingDbContext data, IDealerService dealerService)
        {
            this.data = data;
            this.dealerService = dealerService;
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

            if (query.CategoryId != 0)
            {
                carsQuery = carsQuery.Where(c => c.CategoryId == query.CategoryId);
            }

            var brands = this.data
                        .Cars
                        .Select(c => c.Make)
                        .Distinct()
                        .OrderBy(br => br)
                        .ToList();

            var categories = this.GetCategories();

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
                    Category = c.Category.Name

                }).ToList();

            query.Cars = cars;
            query.TotalCars = totalCars;
            query.Makes = brands;
            query.Categories = categories;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {

            if (!dealerService.UserIsDealer(this.User.GetId()))
            {

                return RedirectToAction(nameof(DealersController.Become ), "Dealers");
            } 
                
            return View(new AddCarFormModel
            {
                Categories = this.GetCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car)
        {
            var dealerId = this.data
                            .Dealers
                            .Where(d => d.UserId == this.User.GetId())
                            .Select(d => d.Id)
                            .FirstOrDefault();

            if (dealerId == 0)
            {
                return RedirectToAction("Create", "Dealers");
            }

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
                CategoryId = car.CategoryId,
                DealerId = dealerId
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
