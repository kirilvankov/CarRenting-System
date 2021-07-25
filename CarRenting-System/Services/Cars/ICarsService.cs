namespace CarRenting_System.Services.Cars
{
    using System.Collections.Generic;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Cars;

    public interface ICarsService
    {
        AllCarsApiServiceModel AllCars(
                            string make,
                            string searchTerm,
                            int categoryId,
                            CarSorting carSorting,
                            int currentPage,
                            int carsPerPage);

        IEnumerable<string> AllCarsBrands();

        IEnumerable<CarCategoryViewModel> GetCategories();
    }
}
