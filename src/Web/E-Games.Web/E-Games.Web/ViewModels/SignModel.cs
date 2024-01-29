using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// Model for signing in/up user profile
    /// </summary>
    public class SignModel
    {
        /// <summary>
        /// User's new Email.
        /// Email is a required field
        /// </summary>
        /// <example>john.doe@example.com</example>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        /// <summary>
        /// User's password. 
        /// Password is a required field, must be at least 8 characters long and have at least one uppercase letter,
        /// a lowercase letter, a digit, and a special character
        /// </summary>
        /// <example>Password1!</example>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and have at least one uppercase letter, " +
            "a lowercase letter, a digit, and a special character.")]
        public string? Password { get; set; }
    }
}
