namespace CarRenting_System.Services.Cars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    using CarRenting_System.Data;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Cars;

    public class CarsService : ICarsService
    {
        private readonly CarRentingDbContext data;

        public CarsService(CarRentingDbContext data) 
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

            var cars = carsQuery
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage)
                .Select(c => new CarServiceModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name

                }).ToList();

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

        public IEnumerable<CarCategoryViewModel> GetCategories()
                => this.data
                    .Categories
                    .Select(c => new CarCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();
    }
}
