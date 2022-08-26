using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Data
{
    public class PriceRating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Value { get; set; } // exqample $, $$, $$$, $$$$
        [Required]
        public string? Text { get; set; } // examples:
                                          // $ = Inexpensive, usually $10 and under
                                          // $$ = Moderately expensive, usually between $10-$25
                                          // $$$ = Expensive, usually between $25-$45
                                          // $$$$ = Very Expensive, usually $50 and up
    }
}
