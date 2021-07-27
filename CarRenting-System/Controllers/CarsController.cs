namespace CarRenting_System.Controllers
{

    using CarRenting_System.Data;
    using CarRenting_System.Data.Infrastucture;
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
                
            return View(new CarFormModel
            {
                Categories = this.carService.GetCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car)
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

            var dealerId = this.dealerService.IdByDealer(userId);

            this.carService.CreateCar(car.Make,
                                    car.Model,
                                    car.Description,
                                    car.Year,
                                    car.ImageUrl,
                                    car.CategoryId,
                                    dealerId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myCars = carService.ByUser(this.User.GetId());

            return View(myCars);
        }

        public IActionResult Details(int id)
        {
            return null;
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.dealerService.UserIsDealer(userId))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            var car = this.carService.Details(id);

            if (car.UserId != userId)
            {
                return Unauthorized();
            }
                        
            return View(new CarFormModel 
            {
                Make = car.Make,
                Model = car.Model,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                Year = car.Year,
                Categories = this.carService.GetCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, CarFormModel car)
        {
            var userId = this.User.GetId();
            var dealerId = this.dealerService.IdByDealer(userId);
            var userIsDealer = this.dealerService.UserIsDealer(userId);

            if (dealerId == 0)
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

            if (!this.carService.CarByDealer(id, dealerId))
            {
                return BadRequest();
            }

            this.carService.EditCar(id, car.Make, car.Model, car.Description, car.Year, car.ImageUrl, car.CategoryId);

            return RedirectToAction(nameof(Mine));
        }

    }
}
