namespace CarRenting_System.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using CarRenting_System.Data;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Cars;

    using Microsoft.AspNetCore.Mvc;
 

    public class HomeController : Controller
    {

        private readonly CarRentingDbContext data;

        public HomeController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .Select(c => new CarListingViewModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,


                })
                .Take(3)
                .ToList();

            return View();
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
