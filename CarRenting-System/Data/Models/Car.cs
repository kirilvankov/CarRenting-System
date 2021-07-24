namespace CarRenting_System.Data.Models
{
    
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Car;
    public class Car
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(MakeMaxLength)]
        public string Make { get; set; }

        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }

        [Required]
        public int DealerId { get; init; }

        public Dealer Dealer { get; init; }
    }
}
