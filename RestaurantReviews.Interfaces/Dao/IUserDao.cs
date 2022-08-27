using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IUserDao
    {
        UserDto? AddUser(UserDto user);
        bool AuthenticateUser(string email, string password);
        void BlockUser(int id);
        void DeleteUser(int id, int currentUserId);
        IEnumerable<UserDto> GetActiveUsers();
        UserDto? GetUser(int id);
        void UnBlockUser(int id);
    }
}
