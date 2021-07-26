namespace CarRenting_System.Services.Dealers
{

    public interface IDealerService
    {
        bool UserIsDealer(string userId);

        int IdByDealer(string userId);
    }
}
