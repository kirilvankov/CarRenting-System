namespace CarRenting_System.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using static DataConstants.Dealer;

    public class Dealer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<Car> Cars { get; init; } = new List<Car>();

    }
}
