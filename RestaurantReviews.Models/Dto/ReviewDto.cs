using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Models.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string? Title { get; set; }
        public string? UserReview { get; set; }
        public int PriceRatingId { get; set; }
        public int StarRatingId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int DeletedByUserId { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
