using System.ComponentModel.DataAnnotations;

namespace RestaurantReviews.Data
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? UserReview { get; set; }
        [Required]
        public int PriceRatingId { get; set; }
        [Required]
        public int StarRatingId { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
