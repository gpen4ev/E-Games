namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// User Profile model
    /// </summary>
    public class UserProfileModel
    {
        /// <summary>
        /// User's email
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// User's phone number
        /// </summary>
        /// <example>123-123-123</example>
        public string? PhoneNumber { get; set; }


        /// <summary>
        /// User's address delivery
        /// </summary>
        /// <example>123 Street Name</example>
        public string? AddressDelivery { get; set; }
    }
}
