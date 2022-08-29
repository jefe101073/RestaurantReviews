using Microsoft.EntityFrameworkCore;

namespace RestaurantReviews.Data
{
    /// <summary>
    /// This helper is used for model building, primarily to seed data with some test data
    /// </summary>
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.com",
                    IsDeleted = false,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("password")
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jeff",
                    LastName = "McCann",
                    Email = "jefe101073@gmail.com",
                    IsDeleted = false,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("password")
                }
            );
            modelBuilder.Entity<PriceRating>().HasData(
                new PriceRating { Id = 1, Text = "$", Value = "Inexpensive, usually $10 and under" },
                new PriceRating { Id = 2, Text = "$$", Value = "Moderately expensive, usually between $10-$25" },
                new PriceRating { Id = 3, Text = "$$$", Value = "Expensive, usually between $25-$45" },
                new PriceRating { Id = 4, Text = "$$$$", Value = "Very Expensive, usually $50 and up" }
                );
            modelBuilder.Entity<StarRating>().HasData(
                new StarRating { Id = 1, Text = "*", Value = "Poor" },
                new StarRating { Id = 2, Text = "**", Value = "Moderate" },
                new StarRating { Id = 3, Text = "***", Value = "Good" },
                new StarRating { Id = 4, Text = "****", Value = "Very Good" },
                new StarRating { Id = 5, Text = "*****", Value = "Excellent" }
                );
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { 
                    Id = 1, 
                    Name = "Primanti Brothers", 
                    Description = "Sandwich Shop", 
                    Address1 = "46 18th Street", 
                    City="Pittsburgh", 
                    State = "PA", 
                    PostalCode = "15222",
                    AveragePriceRating = 2,
                    AverageStarRating = 4
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Pizza Hut",
                    Description = "Pizza and Wings",
                    Address1 = "1279 Camp Horne Road",
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15237",
                    AveragePriceRating = 1,
                    AverageStarRating = 3
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "Willow",
                    Description = "Upscale-casual eatery featuring modern American dishes including crab cakes, beef tenderloin & veal.",
                    Address1 = "634 Camp Horne Road",
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15237",
                    AveragePriceRating = 3,
                    AverageStarRating = 5
                }
            );
            modelBuilder.Entity<Review>().HasData(
                new Review { 
                    Id = 1, 
                    UserId = 1,
                    Title = "A Real Pittsburgh Tradition", 
                    UserReview = "Best sandwiches with cole slaw and fries in the world.", 
                    RestaurantId = 1,
                    PriceRatingId = 2,
                    StarRatingId = 4
                },
                new Review
                {
                    Id = 2,
                    UserId = 1,
                    Title = "No wonder Pizza Hut has so many locations!",
                    UserReview = "Consistant quality and very affordable.",
                    RestaurantId = 2,
                    PriceRatingId = 1,
                    StarRatingId = 3
                },
                new Review
                {
                    Id = 3,
                    UserId = 1,
                    Title = "I never knew steak could taste so good!",
                    UserReview = "Wonderful service, the food is top-notch, would recommend for anyone who needs a special occasion.",
                    RestaurantId = 3,
                    PriceRatingId = 3,
                    StarRatingId = 5
                }
                );
        }
    }
}
