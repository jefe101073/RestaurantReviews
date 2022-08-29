using RestaurantReviews.Data;
using RestaurantReviews.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Tests.DataForTests
{
    public static class TestData
    {
        public static List<RestaurantDto> RestaurantList;
        public static List<ReviewDto> ReviewList;
        public static List<UserDto> UserList;

        static TestData()
        {
            RestaurantList = new List<RestaurantDto>();
            ReviewList = new List<ReviewDto>();
            UserList = new List<UserDto>();
        }

        public static void LoadData()
        {
            RestaurantList = new List<RestaurantDto>
            {
                new RestaurantDto()
                {
                    Id = 1,
                    Name = "McDonald's",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 McDonald Road",
                    Address2 = "Suite 4",
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15237",
                    IsDeleted = false,
                    PriceRatingId = 1,
                    StarRatingId = 1,
                    DeletedByUserId = null,
                    DeletedOn = null
                },
                new RestaurantDto()
                {
                    Id = 2,
                    Name = "Burger King",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 BK Road",
                    Address2 = null,
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15239",
                    IsDeleted = false,
                    PriceRatingId = 1,
                    StarRatingId = 1,
                    DeletedByUserId = null,
                    DeletedOn = null
                },
                new RestaurantDto()
                {
                    Id = 3,
                    Name = "White Castle",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 White Castle Road",
                    Address2 = null,
                    City = "Indianapolis",
                    State = "IN",
                    PostalCode = "46260",
                    IsDeleted = true,
                    PriceRatingId = 1,
                    StarRatingId = 5,
                    DeletedByUserId = 1,
                    DeletedOn = DateTime.UtcNow
                }
            };

            ReviewList = new List<ReviewDto>
            {
                new ReviewDto
                {
                    Id = 1,
                    UserId = 1,
                    RestaurantId = 1,
                    Title = "McDonalds is quck and easy.",
                    UserReview = "standard fast food",
                    PriceRatingId = 1,
                    StarRatingId = 2,
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOn = null
                },
                new ReviewDto
                {
                    Id = 2,
                    UserId = 1,
                    RestaurantId = 2,
                    Title = "Burger King uses FIRE!",
                    UserReview = "flamed grilled to perfection",
                    PriceRatingId = 1,
                    StarRatingId = 3,
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOn = null
                },
                new ReviewDto
                {
                    Id = 3,
                    UserId = 1,
                    RestaurantId = 3,
                    Title = "Poor White Castle :(",
                    UserReview = "Wish they had more locations",
                    PriceRatingId = 1,
                    StarRatingId = 3,
                    IsDeleted = true,
                    DeletedByUserId = 1,
                    DeletedOn = DateTime.UtcNow
                },
            };

            UserList = new List<UserDto>
            {
                new UserDto()
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.com",
                    IsDeleted = false,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("password")
                },
                new UserDto()
                {
                    Id = 2,
                    FirstName = "Jeff",
                    LastName = "McCann",
                    Email = "jefe101073@gmail.com",
                    IsDeleted = false,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("password")
                },
                new UserDto()
                {
                    Id = 3,
                    FirstName = "Deleted",
                    LastName = "McDeleterson",
                    Email = "deleted@gmail.com",
                    IsDeleted = true,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("deleted")
                }
            };

        }
    }
}
