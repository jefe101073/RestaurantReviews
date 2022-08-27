using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IReviewDao
    {
        ReviewDto? AddReview(ReviewDto user);
        void DeleteReview(int id, int currentUserId);
        IEnumerable<ReviewDto> GetActiveReviews();
        ReviewDto? GetReview(int id);
    }
}
