namespace RestaurantReviews.Models.Dto
{
    /// <summary>
    /// This DTO is meant to be helpful for sending an initial review where both the restaurant and review need to be added
    /// This will also be used in some update functionality for ease of use
    /// </summary>
    public class RestaurantAndReviewDto
    {
        public RestaurantDto? Restaurant { get; set; }
        public ReviewDto? Review { get; set; }
    }
}
