namespace CarRenting_System.Services.Dealers
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

        public int IdByDealer(string userId)
            => this.data
            .Dealers
            .Where(d => d.UserId == userId)
            .Select(d => d.Id)
            .FirstOrDefault();

        public bool UserIsDealer(string userId)
            => this.data
                   .Dealers
                   .Any(d => d.UserId == userId);

    }
}
