namespace CarRenting_System.Models.Cars
{
    using System;

    public class CarListingViewModel
    {
        public int Id { get; init; }

        public string Make { get; set; }
       
        public string Model { get; set; }
        
        public int Year { get; set; }
       
        public string ImageUrl { get; set; }

        public string Category { get; set; }
    }
}
