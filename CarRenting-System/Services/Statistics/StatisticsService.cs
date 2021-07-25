namespace CarRenting_System.Services.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CarRenting_System.Data;

    public class StatisticsService : IStatisticsService
    {
        private readonly CarRentingDbContext data;

        public StatisticsService(CarRentingDbContext data)
            => this.data = data;

        public StatisticsServiceModel GetStatistics()
        {
            var statistics = new StatisticsServiceModel
            {
                TotalCars = this.data.Cars.Count(),
                TotalUsers = this.data.Users.Count(),
                TotalRents = 0
            };

            return statistics;
        }
    }
}
