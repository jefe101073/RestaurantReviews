using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
