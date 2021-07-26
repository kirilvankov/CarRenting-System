namespace CarRenting_System.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    
    using CarRenting_System.Data;
    using CarRenting_System.Data.Infrastucture;
    using CarRenting_System.Data.Models;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Cars;
    using CarRenting_System.Services.Dealers;
    using CarRenting_System.Services.Cars;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IDealerService dealerService;
        private readonly ICarsService carService;

        public CarsController(CarRentingDbContext data, IDealerService dealerService, ICarsService carService)
        {
            this.data = data;
            this.dealerService = dealerService;
            this.carService = carService;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryCars = this.carService.AllCars(
                        query.Make,
                        query.SearchTerm,
                        query.CategoryId,
                        query.Sorting,
                        query.CurrentPage,
                        AllCarsQueryModel.CarsPerPage);

            var brands = this.carService.AllCarsBrands();

            var categories = this.carService.GetCategories();

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
                Categories = this.carService.GetCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car)
        {
            var userId = this.User.GetId();
             
            var userIsDealer = this.dealerService.UserIsDealer(userId);

            if (!userIsDealer)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }
            

            if (!this.carService.CategoryExists(car.CategoryId))
            {
                ModelState.AddModelError("Category", "Invalid category");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.carService.GetCategories(); 
                return View(car);
            }

            var dealerId = this.data.Dealers.Where(d => d.UserId == userId).Select(d => d.Id).FirstOrDefault();
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

        [Authorize]
        public IActionResult Mine()
        {
            var myCars = carService.ByUser(this.User.GetId());
            return View(myCars);
        }

    }
}
