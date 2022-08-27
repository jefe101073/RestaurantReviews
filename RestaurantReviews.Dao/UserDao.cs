using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System.Data.Entity.Core;

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
            if (user == null || user.Password == null)
            {
                return null;
            }
            // Check for duplicate email address
            var emailCheck = _context.Users.FirstOrDefault(z => z.Email == user.Email);
            if (emailCheck != null)
            {
                throw new ArgumentException($"Error.  Email address already exists in the system.{user.Email}", nameof(user.Email));
            }

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
            _context.SaveChanges();
            return user;
        }

        // Allows user to login based on email/password.  Returns true if authentication passes.
        public bool AuthenticateUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(e => e.Email == email);
            if (user == null || user.Password == null)
            {
                return false;
            }

            var passwordDecrypted = DataHelpers.PasswordDecrypt(user.Password);
            if (string.IsNullOrEmpty(passwordDecrypted))
            {
                return false;
            }

            return password.Equals(passwordDecrypted, StringComparison.Ordinal);
        }

        // Mark the IsUserBlocked flag and return void
        public void BlockUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsUserBlocked = true;
            _context.SaveChanges();
        }

        // Mark the deleted flag as true for the given user and return void
        public void DeleteUser(int id, int currentUserId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            // Can't delete user if doesn't exist
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsDeleted = true;
            user.DeletedByUserId = currentUserId;
            user.DeletedOn = DateTime.Now;
            _context.SaveChanges();
        }

        // Get Users who have not been deleted
        public IEnumerable<UserDto> GetActiveUsers()
        {
            var users = from user in _context.Users
                        where user.IsDeleted == false
                        select new UserDto
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Password = user.Password,
                            IsUserBlocked = user.IsUserBlocked,
                            IsDeleted = user.IsDeleted,
                            DeletedByUserId = user.DeletedByUserId,
                            DeletedOn = user.DeletedOn
                        };

            return users;
        }

        // Get User by Id
        public UserDto? GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(z => z.Id == id);
            if (user == null)
            {
                return null;
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                IsUserBlocked = user.IsUserBlocked,
                IsDeleted = user.IsDeleted,
                DeletedByUserId = user.DeletedByUserId,
                DeletedOn = user.DeletedOn
            };
            return userDto;
        }

        public void UnBlockUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsUserBlocked = false;
            _context.SaveChanges();
        }
    }
}