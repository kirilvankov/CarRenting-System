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

        bool CategoryExists(int categoryId);
        CarDetailServiceModel Details(int id);

        IEnumerable<CarServiceModel> ByUser(string userId);
        bool CarByDealer(int id, int dealerId);

        bool EditCar(int id, string make, string model, string description, int year, string imageUrl, int categoryId);
        void CreateCar(string make, string model, string description, int year, string imageUrl, int categoryId, int dealerId);

        IEnumerable<CarCategoryServiceModel> GetCategories();
    }
}
