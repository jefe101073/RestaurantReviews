using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Dao
{
    /// <summary>
    /// This class exposes the User Data to the API through Data Access Objects and returns Data Transfer Objects to keep the API clean from direct Data calls
    /// </summary>
    public class UserDao : IUserDao
    {
        private readonly RestaurantReviewDataContext _context;
        public UserDao(RestaurantReviewDataContext context)
        {
            _context = context;
        }

        // Add user to the database
        public UserDto? AddUser(UserDto user)
        {
            if(user == null) return null;
            var userObj = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = DataHelpers.PasswordEncrypt(user.Password),
                IsUserBlocked = user.IsUserBlocked,
                IsDeleted = user.IsDeleted,
                DeletedByUserId = user.DeletedByUserId,
                DeletedOn = user.DeletedOn
            };
            _context.Users.Add(userObj);
            return user;
        }

        // Mark the deleted flag as true for the given user and return void
        public void DeleteUser(int id, int currentUserId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return;
            user.IsDeleted = true;
            user.DeletedByUserId = currentUserId;
            user.DeletedOn = DateTime.Now;
            _context.SaveChanges();
        }

        // Get Users who have not been deleted
        public IEnumerable<UserDto> GetActiveUsers()
        {
            var users = from u in _context.Users
                        where u.IsDeleted == false
                        select new UserDto
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Password = u.Password,
                            IsUserBlocked = u.IsUserBlocked,
                            IsDeleted = u.IsDeleted,
                            DeletedByUserId = u.DeletedByUserId,
                            DeletedOn = u.DeletedOn
                        };

            return users;
        }

        // Get User by Id
        public UserDto? GetUser(int id)
        {
            var u = _context.Users.FirstOrDefault(z => z.Id == id);
            if (u == null) return null;

            var userDto = new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                IsUserBlocked = u.IsUserBlocked,
                IsDeleted = u.IsDeleted,
                DeletedByUserId = u.DeletedByUserId,
                DeletedOn = u.DeletedOn
            };
            return userDto;

        }
    }
}