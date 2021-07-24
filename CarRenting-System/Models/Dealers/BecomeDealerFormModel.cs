namespace CarRenting_System.Models.Dealers
{
    
    using System.ComponentModel.DataAnnotations;
    
    using static Data.DataConstants.Dealer;

    public class BecomeDealerFormModel
    {
        [Required]
        [Display(Name ="Full Name")]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; set; }

    }
}
