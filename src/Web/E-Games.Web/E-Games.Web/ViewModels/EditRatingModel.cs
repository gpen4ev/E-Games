using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// Model for rating editing
    /// </summary>
    public class EditRatingModel
    {
        /// <summary>
        /// Name of a game. 
        /// GameName is a required field.
        /// </summary>
        /// <example>FIFA 2024</example>
        [Required]
        public string? GameName { get; set; }

        /// <summary>
        /// New rating of a game. 
        /// NewRating is a required field.
        /// Ratings are expected to be within the range of 1 to 5.
        /// </summary>
        /// <example>5</example>
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int NewRating { get; set; }
    }
}
