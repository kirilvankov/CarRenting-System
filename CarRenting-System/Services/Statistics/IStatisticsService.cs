namespace CarRenting_System.Services.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IStatisticsService
    {
        StatisticsServiceModel GetStatistics();
    }
}
