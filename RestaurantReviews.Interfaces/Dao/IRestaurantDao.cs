using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IRestaurantDao
    {
        RestaurantDto? AddRestaurant(RestaurantDto user);
        void DeleteRestaurant(int id, int currentUserId);
        IEnumerable<RestaurantDto> GetActiveRestaurants();
        RestaurantDto? GetRestaurant(int id);
    }
}
