using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Data
{
    /// <summary>
    /// This class sets up the DbContext to be used in Entity Framework Core
    /// </summary>
    public class RestaurantReviewDataContext : DbContext
    {
        public RestaurantReviewDataContext(DbContextOptions<RestaurantReviewDataContext> options):
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.Seed();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PriceRating> PriceRatings { get; set; }
        public DbSet<StarRating> StarRatings { get; set; }

    }
}
