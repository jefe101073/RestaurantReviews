using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IUserDao
    {
        Task<UserDto?> AddUserAsync(AddUserDto user);
        Task<bool> AuthenticateUserAsync(string email, string password);
        Task BlockUserAsync(int id);
        Task DeleteUserAsync(int id, int currentUserId);
        Task<IEnumerable<UserDto>> GetActiveUsersAsync();
        Task<UserDto?> GetUserAsync(int id);
        Task<bool> IsUserBlockedOrDeletedAsync(int userId);
        Task UnBlockUserAsync(int id);
        Task UndeleteUserAsync(int id);
    }
}
