namespace CarRenting_System.Services.Cars
{

    using System.Linq;
    using System.Collections.Generic;

    using CarRenting_System.Data;
    using CarRenting_System.Data.Models;
    using CarRenting_System.Models;

    public class CarService : ICarsService
    {
        private readonly CarRentingDbContext data;

        public CarService(CarRentingDbContext data)
            => this.data = data;

        public AllCarsApiServiceModel AllCars(
                                string make,
                                string searchTerm,
                                int categoryId,
                                CarSorting carSorting,
                                int currentPage,
                                int carsPerPage)
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(make))
            {
                carsQuery = carsQuery.Where(c => c.Make == make);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                        (c.Make + " " + c.Model).ToLower().Contains(searchTerm.ToLower()) ||
                         c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            if (categoryId != 0)
            {
                carsQuery = carsQuery.Where(c => c.CategoryId == categoryId);
            }

            carsQuery = carSorting switch
            {
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.MakeAndModel => carsQuery.OrderBy(c => c.Make).ThenBy(c => c.Model),
                CarSorting.DateCreated or _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = this.GetCars(carsQuery
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage));

            return new AllCarsApiServiceModel
            {
                TotalCars = totalCars,
                Cars = cars,
                CurrentPage = currentPage,
                CarsPerPage = carsPerPage
            };
        }

        public IEnumerable<string> AllCarsBrands()
                => this.data
                        .Cars
                        .Select(c => c.Make)
                        .Distinct()
                        .OrderBy(br => br)
                        .ToList();

        public IEnumerable<CarServiceModel> ByUser(string userId)
            => this.GetCars(this.data
                .Cars
                .Where(c => c.Dealer.UserId == userId));

        public bool CarByDealer(int id, int dealerId) 
            => this.data
                .Cars
                .Any(c => c.Id == id && c.DealerId == dealerId);

        public bool CategoryExists(int categoryId)
            => this.data
                .Categories
                .Any(c => c.Id == categoryId);

        public void CreateCar(string make, string model, string description, int year, string imageUrl, int categoryId, int dealerId)
        {
            var carData = new Car
            {
                Make = make,
                Model = model,
                Year = year,
                Description = description,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                DealerId = dealerId
            };

            this.data.Cars.Add(carData);
            this.data.SaveChanges();
        }

        public CarDetailServiceModel Details(int id)
            => this.data
            .Cars
            .Where(c => c.Id == id)
            .Select(c => new CarDetailServiceModel
            {
                Make = c.Make,
                Model = c.Model,
                Description = c.Description,
                Year = c.Year,
                ImageUrl = c.ImageUrl,
                CategoryId = c.CategoryId,
                CategoryName = c.Category.Name,
                DealerId = c.DealerId,
                UserId = c.Dealer.UserId
            }).FirstOrDefault();


        public bool EditCar(int id, string make, string model, string description, int year, string imageUrl, int categoryId)
        {
            var car = this.data.Cars.Find(id);

            if (car == null)
            {
                return false;
            }

            car.Make = make;
            car.Model = model;
            car.Description = description;
            car.Year = year;
            car.ImageUrl = imageUrl;
            car.CategoryId = categoryId;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<CarCategoryServiceModel> GetCategories()
                => this.data
                    .Categories
                    .Select(c => new CarCategoryServiceModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

        private IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carsQuery)
            => carsQuery
            .Select(c => new CarServiceModel
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                ImageUrl = c.ImageUrl,
                Year = c.Year,
                CategoryName = c.Category.Name

            }).ToList();
    }
}
