using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IReviewDao
    {
        Task<RestaurantAndReviewDto?> AddRestaurantAndReviewAsync(AddRestaurantAndReviewDto restaurantAndReview);
        Task<ReviewDto?> AddReviewAsync(AddReviewDto user);
        Task CalculateAndUpdateRestaurantRatingsAsync(int restaurantId, int priceRatingId, int starRatingId);
        Task DeleteReviewAsync(int id, int currentUserId);
        Task<IEnumerable<ReviewDto>> GetActiveReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetActiveReviewsByRestaurantAsync(int restaurantId);
        Task<IEnumerable<ReviewDto>?> GetActiveReviewsByRestaurantNameAndCityAsync(string name, string city);
        Task<IEnumerable<ReviewDto>> GetActiveReviewsByUserAsync(int userId);
        Task<ReviewDto?> GetReviewAsync(int id);
    }
}
