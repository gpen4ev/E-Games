using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    public class SignModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and have at least one uppercase letter, " +
            "a lowercase letter, a digit, and a special character.")]
        public string? Password { get; set; }
    }
}
