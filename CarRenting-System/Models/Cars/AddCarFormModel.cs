namespace CarRenting_System.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CarRenting_System.Services.Cars;

    using static Data.DataConstants.Car;
    public class AddCarFormModel
    {
        [Required]
        [StringLength(MakeMaxLength, MinimumLength = MakeMinLength)]
        public string Make { get; set; }

        [Required]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength)]
        public string Model { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage ="You should enter the description with maximum {1} characters long.")]
        public string Description { get; set; }

        [Range(YearMinValue, YearMaxValue, ErrorMessage ="Car year should be between {1} and {2}.")]
        public int Year { get; set; }

        [Display(Name ="Image Url")]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CarCategoryServiceModel> Categories { get; set; }
    }
}
