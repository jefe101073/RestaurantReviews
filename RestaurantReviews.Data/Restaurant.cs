using System.ComponentModel.DataAnnotations;

namespace RestaurantReviews.Data
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? PostalCode { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public double? AveragePriceRating { get; set; } // Calculated value based on reviewer's estimate, value will update when review is added.
        public double? AverageStarRating { get; set; } // Calculated value based on average reviews, value will update when review is added.
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
