using RestaurantReviews.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Interfaces.Dao
{
    public interface IUserDao
    {
        UserDto? AddUser(UserDto user);
        void DeleteUser(int id, int currentUserId);
        IEnumerable<UserDto> GetActiveUsers();
        UserDto? GetUser(int id);
    }
}
