using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System.Data.Entity;
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
        public async Task<UserDto?> AddUserAsync(AddUserDto user)
        {
            if (user == null || user.Password == null)
            {
                return null;
            }
            // Check for duplicate email address
            var emailCheck = await _context.Users.FirstOrDefaultAsync(z => z.Email == user.Email);
            if (emailCheck != null)
            {
                throw new ArgumentException($"Error.  Email address already exists in the system.{user.Email}", nameof(user.Email));
            }

            using (var context = _context)
            {
                var userObj = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = DataHelpers.PasswordEncrypt(user.Password),
                    IsUserBlocked = user.IsUserBlocked,
                    IsDeleted = false
                };
                await _context.Users.AddAsync(userObj);
                await _context.SaveChangesAsync();
                return new UserDto
                {
                    Id = userObj.Id,
                    FirstName = userObj.FirstName,
                    LastName = userObj.LastName,
                    Email = userObj.Email,
                    Password = userObj.Password,
                    IsUserBlocked = userObj.IsUserBlocked,
                    IsDeleted = false
                };
            }
        }

        // Allows user to login based on email/password.  Returns true if authentication passes.
        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
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
        public async Task BlockUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsUserBlocked = true;
            await _context.SaveChangesAsync();
        }

        // Mark the deleted flag as true for the given user and return void
        public async Task DeleteUserAsync(int id, int currentUserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            // Can't delete user if doesn't exist
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsDeleted = true;
            user.DeletedByUserId = currentUserId;
            user.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Get Users who have not been deleted
        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
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
            return await users.ToListAsync();
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(z => z.Id == id);
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

        public async Task<bool> IsUserBlockedOrDeletedAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(z => z.Id == userId);
            if (user == null)
            {
                throw new ArgumentException($"Error.  User does not exist in the system.{userId}", nameof(userId));
            }
            return user.IsDeleted || user.IsUserBlocked;
        }

        public async Task UnBlockUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsUserBlocked = false;
            await _context.SaveChangesAsync();
        }

        public async Task UndeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            user.IsDeleted = false;
            user.DeletedOn = null;
            user.DeletedByUserId = null;
            await _context.SaveChangesAsync();
        }
    }
}