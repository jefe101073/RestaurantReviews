using System.ComponentModel.DataAnnotations;

namespace RestaurantReviews.Data
{
    /// <summary>
    /// This table will store values for the restaurant star rating.  Each review will give the ratings of 1-5 stars.
    /// </summary>
    public class StarRating
    {
        [Key]
        public int Id { get; set; }
        public string? Value { get; set; } // example *, **, ***, ****, *****
        [Required]
        public string? Text { get; set; } // examples:
                                          // * = Poor
                                          // ** = Moderate
                                          // *** = Good
                                          // **** = Very Good
                                          // ***** = Excellent
    }
}
