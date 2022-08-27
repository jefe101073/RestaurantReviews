namespace RestaurantReviews.Models.Dto
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int PriceRatingId { get; set; } // Calculated value based on reviewer's estimate, value will update when review is added.
        public int StarRatingId { get; set; } // Calculated value based on average reviews, value will update when review is added.
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
