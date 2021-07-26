namespace CarRenting_System.Services.Cars
{

    public class CarServiceModel
    {
        public int Id { get; init; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

    }
}
