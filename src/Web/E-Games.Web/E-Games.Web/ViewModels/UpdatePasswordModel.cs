using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    public class UpdatePasswordModel
    {
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and have at least one uppercase letter, " +
            "a lowercase letter, a digit, and a special character.")]
        public string? NewPassword { get; set; }
    }
}
