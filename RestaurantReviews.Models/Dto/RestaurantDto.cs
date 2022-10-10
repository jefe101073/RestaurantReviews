namespace RestaurantReviews.Models.Dto
{
    /// <summary>
    /// The smaller AddRestaurantDto does not include Id, or database nullable fields, simplifies the Add method
    /// </summary>
    public class AddRestaurantDto // used for adding new restaurants, ID is auto-generated and deletedBy & deletedOn are null for first time creating
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
    public class RestaurantDto : AddRestaurantDto
    {
        public int Id { get; set; }
        public double? AveragePriceRating { get; set; } // Null when created, Calculated value based on reviewer's estimate, value will update when review is added.
        public double? AverageStarRating { get; set; } // Null when created, Calculated value based on average reviews, value will update when review is added.
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
