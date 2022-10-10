using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IRestaurantDao
    {
        Task<RestaurantDto?> AddRestaurantAsync(AddRestaurantDto restaurant);
        Task DeleteRestaurantAsync(int id, int currentUserId);
        Task<IEnumerable<RestaurantDto>> GetActiveRestaurantsAsync();
        Task<IEnumerable<RestaurantDto>?> GetActiveRestaurantsByCityAsync(string city);
        Task <RestaurantDto?> GetRestaurantAsync(int id);
    }
}
