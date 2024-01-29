using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// Model for updating user profile
    /// </summary>
    public class UpdateUserModel
    {
        /// <summary>
        /// User's new username.
        /// UserName is a required field
        /// </summary>
        /// <example>user123</example>
        [Required]
        public string? UserName { get; set; }

        /// <summary>
        /// User's new phone number. 
        /// PhoneNumber is a required field.
        /// </summary>
        /// <example>124-124-124</example>
        [Required]
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$",
            ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// User's new address delivery. 
        /// AddressDelivery is a required field.
        /// </summary>
        /// <example>124 Street Name</example>
        [Required]
        public string? AddressDelivery { get; set; }
    }
}
