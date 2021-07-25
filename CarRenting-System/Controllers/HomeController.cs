namespace CarRenting_System.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using CarRenting_System.Data;
    using CarRenting_System.Models;
    using CarRenting_System.Models.Home;
    using CarRenting_System.Services.Statistics;

    using Microsoft.AspNetCore.Mvc;
 

    public class HomeController : Controller
    {

        private readonly CarRentingDbContext data;
        private readonly IStatisticsService statistics;

        public HomeController(CarRentingDbContext data, IStatisticsService statistics)
        {
            this.data = data;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            
            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .Select(c => new CarIndexViewModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                })
                .Take(3)
                .ToList();

            var totalStatistics = this.statistics.GetStatistics();
            return View(new IndexViewModel 
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
                //TODO: Add total rents!
                Cars = cars,

            });
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
