namespace RestaurantReviews.Models.Dto
{
    /// <summary>
    /// The smaller AddReviewDto does not include Id, or database nullable fields, simplifies the Add method
    /// </summary>
    public class AddReviewDto // used for adding new restaurants, ID is auto-generated and deletedBy & deletedOn are null for first time creating
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string? Title { get; set; }
        public string? UserReview { get; set; }
        public int PriceRatingId { get; set; }
        public int StarRatingId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
    public class ReviewDto : AddReviewDto
    {
        public int Id { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
