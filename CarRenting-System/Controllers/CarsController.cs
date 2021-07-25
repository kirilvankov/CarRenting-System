namespace CarRenting_System.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    
    using CarRenting_System.Data;
    using CarRenting_System.Data.Infrastucture;
    using CarRenting_System.Data.Models;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Cars;
    using CarRenting_System.Services;
    using CarRenting_System.Services.Cars;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IDealerService dealerService;
        private readonly ICarsService carsService;

        public CarsController(CarRentingDbContext data, IDealerService dealerService, ICarsService carsService)
        {
            this.data = data;
            this.dealerService = dealerService;
            this.carsService = carsService;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryCars = this.carsService.AllCars(
                        query.Make,
                        query.SearchTerm,
                        query.CategoryId,
                        query.Sorting,
                        query.CurrentPage,
                        AllCarsQueryModel.CarsPerPage);

            var brands = this.carsService.AllCarsBrands();

            var categories = this.carsService.GetCategories();

            query.Cars = queryCars.Cars;
            query.TotalCars = queryCars.TotalCars;
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
                Categories = this.carsService.GetCategories()
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
                car.Categories = this.carsService.GetCategories(); 
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

        
            
            

    }
}
