namespace CarRenting_System.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    public class AddCarFormModel
    {
        [Required]
        [StringLength(CarMakeMaxLength, MinimumLength = CarMakeMinLength)]
        public string Make { get; set; }

        [Required]
        [StringLength(CarModelMaxLength, MinimumLength = CarModelMinLength)]
        public string Model { get; set; }

        [Required]
        [StringLength(CarDescriptionMaxLength, MinimumLength = CarDescriptionMinLength, ErrorMessage ="You should enter the description with maximum {1} characters long.")]
        public string Description { get; set; }

        [Range(CarYearMinValue, CarYearMaxValue, ErrorMessage ="Car year should be between {1} and {2}.")]
        public int Year { get; set; }

        [Display(Name ="Image Url")]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CarCategoryViewModel> Categories { get; set; }
    }
}
