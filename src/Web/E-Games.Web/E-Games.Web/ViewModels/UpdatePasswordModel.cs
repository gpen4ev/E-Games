using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// Model for updating user password
    /// </summary>
    public class UpdatePasswordModel
    {
        /// <summary>
        /// User's current password
        /// </summary>
        /// <example>currentPassword123</example>
        public string? CurrentPassword { get; set; }

        /// <summary>
        /// User's new password. 
        /// NewPassword is a required field, must be at least 8 characters long and have at least one uppercase letter,
        /// a lowercase letter, a digit, and a special character
        /// </summary>
        /// <example>newpassword123</example>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and have at least one uppercase letter, " +
            "a lowercase letter, a digit, and a special character.")]
        public string? NewPassword { get; set; }
    }
}
