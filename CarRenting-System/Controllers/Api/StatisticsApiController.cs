namespace CarRenting_System.Controllers.Api
{
    using System.Linq;

    using CarRenting_System.Data;
    using CarRenting_System.Models.Api.Statistics;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;

        public StatisticsApiController(CarRentingDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var statistics = new StatisticsResponseModel
            {
                TotalCars = this.data.Cars.Count(),
                TotalUsers = this.data.Users.Count(),
                TotalRents = 0
            };

            return statistics;
        }
    }
}
