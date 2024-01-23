using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    public class UpdateUserModel
    {
        public string? UserName { get; set; }

        [Required]
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$",
            ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? AddressDelivery { get; set; }
    }
}
