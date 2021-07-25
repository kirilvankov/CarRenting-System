namespace CarRenting_System.Services
{
    using System.Linq;
    
    using CarRenting_System.Data;

    public class DealerService : IDealerService
    {
        private readonly CarRentingDbContext data;

        public DealerService(CarRentingDbContext data)
        {
            this.data = data;
        }

        public bool UserIsDealer(string userId)
            => this.data
                   .Dealers
                   .Any(d => d.UserId == userId);
    }
}
