namespace CarRenting_System.Controllers.Api
{

    using CarRenting_System.Models.Api.Statistics;
    using CarRenting_System.Services.Statistics;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService statistics)
        {
            this.statistics = statistics;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var totalStatistics = this.statistics.GetStatistics();

            return new StatisticsResponseModel
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
                TotalRents = totalStatistics.TotalRents
            };

        }
    }
}
