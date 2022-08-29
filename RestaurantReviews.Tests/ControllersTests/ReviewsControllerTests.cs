using RestaurantReviews.API.Controllers;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using RestaurantReviews.Tests.DataForTests;

namespace RestaurantReviews.Tests.ControllersTests
{
    /// <summary>
    /// This test class tests the Restaurants controller and the functionality in the Dao that the controller directly accesses.
    /// </summary>
    [TestClass]
    public class ReviewsControllerTests
    {
        private Mock<IReviewDao> _reviewDaoMock;
        private Mock<IUserDao> _userDaoMock;

        public ReviewsControllerTests()
        {
            _reviewDaoMock = new Mock<IReviewDao>();
            _userDaoMock = new Mock<IUserDao>();
        }

        // Creates an instance of the controller so we can test
        private ReviewsController GetControllerInstance()
        {
            return new ReviewsController(_reviewDaoMock.Object);
        }

        // runs every time a test starts
        [TestInitialize()]
        public void TestInitialize()
        {
            TestData.LoadData(); // Reset the test data
            // Mock functionality
            _reviewDaoMock.Setup(x => x.GetActiveReviewsAsync()).ReturnsAsync(TestData.ReviewList.Where(u => u.IsDeleted == false));

            _reviewDaoMock.Setup(x => x.GetActiveReviewsByUserAsync(It.IsAny<int>())).ReturnsAsync(
                (int id) =>
                {
                    return TestData.ReviewList.Where(x => x.IsDeleted == false && x.Id == id);
                });

            _reviewDaoMock.Setup(x => x.GetActiveReviewsByRestaurantAsync(It.IsAny<int>())).ReturnsAsync(
                (int id) =>
                {
                    return TestData.ReviewList.Where(x => x.IsDeleted == false && x.RestaurantId == id);
                });

            _reviewDaoMock.Setup(x => x.GetActiveReviewsByRestaurantNameAndCityAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                (string name, string city) =>
                {
                    var query = from rest in TestData.RestaurantList
                                join review in TestData.ReviewList on rest.Id equals review.RestaurantId
                                where rest.Name != null && rest.Name.ToLower() == name.ToLower() && rest.City != null && rest.City.ToLower() == city.ToLower()
                                select review;

                    return query;
                });

            _reviewDaoMock.Setup(x => x.AddReviewAsync(It.IsAny<ReviewDto>())).ReturnsAsync(
                (ReviewDto target) =>
                {
                    TestData.ReviewList.Add(target);
                    return target;
                });

            _reviewDaoMock.Setup(x => x.AddRestaurantAndReviewAsync(It.IsAny<AddRestaurantAndReviewDto>())).ReturnsAsync(
                (AddRestaurantAndReviewDto target) =>
                {
                    var restNextId = TestData.RestaurantList.Count + 1;
                    var revNextId = TestData.ReviewList.Count + 1;
                    var restaurant = new RestaurantDto
                    {
                        Id = restNextId,
                        Name = target.Restaurant.Name,
                        Description = target.Restaurant.Description,
                        Address1 = target.Restaurant.Address1,
                        Address2 = target.Restaurant.Address2,
                        City = target.Restaurant.City,
                        State = target.Restaurant.State,
                        PostalCode = target.Restaurant.PostalCode,
                        IsDeleted = target.Restaurant.IsDeleted,
                        DeletedByUserId = null,
                        DeletedOn = null
                    };
                    TestData.RestaurantList.Add(restaurant);
                    var review = new ReviewDto
                    {
                        Id = revNextId,
                        UserId = target.Review.UserId,
                        RestaurantId = restNextId,
                        Title = target.Review.Title,
                        UserReview = target.Review.UserReview,
                        PriceRatingId = target.Review.PriceRatingId,
                        StarRatingId = target.Review.StarRatingId,
                        IsDeleted = target.Review.IsDeleted,
                        DeletedByUserId = null,
                        DeletedOn = null
                    };
                    TestData.ReviewList.Add(review);
                    var restaurantAndReview = new RestaurantAndReviewDto();
                    restaurantAndReview.Restaurant = restaurant;
                    restaurantAndReview.Review = review;
                    return restaurantAndReview;
                });
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            // reset mocks to prepare for the next test
            _reviewDaoMock.Reset();
            _userDaoMock.Reset();
        }

        [TestMethod]
        public async Task CanGetActiveReviewsAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedReviews = new List<ReviewDto>()
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
                }
            };

            // Act
            var reviews = await controller.GetActiveReviewsAsync();
            var actualReviews = reviews.ToList();

            // Assert
            Assert.IsNotNull(actualReviews);
            Assert.AreEqual(expectedReviews.Count, actualReviews.Count());
            for (int i = 0; i < expectedReviews.Count; i++)
            {
                Assert.AreEqual(expectedReviews[i].Id, actualReviews[i].Id);
                Assert.AreEqual(expectedReviews[i].UserId, actualReviews[i].UserId);
                Assert.AreEqual(expectedReviews[i].RestaurantId, actualReviews[i].RestaurantId);
                Assert.AreEqual(expectedReviews[i].Title, actualReviews[i].Title);
                Assert.AreEqual(expectedReviews[i].UserReview, actualReviews[i].UserReview);
                Assert.AreEqual(expectedReviews[i].PriceRatingId, actualReviews[i].PriceRatingId);
                Assert.AreEqual(expectedReviews[i].StarRatingId, actualReviews[i].StarRatingId);
                Assert.AreEqual(expectedReviews[i].IsDeleted, actualReviews[i].IsDeleted);
            }
        }

        [TestMethod]
        public async Task CanGetActiveReviewsByRestaurantAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedReviews = new List<ReviewDto>()
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
                }
            };

            // Act
            var reviews = await controller.GetActiveReviewsByRestaurantAsync(1);
            var actualReviews = reviews.ToList();

            // Assert
            Assert.IsNotNull(actualReviews);
            Assert.AreEqual(expectedReviews.Count, actualReviews.Count());
            for (int i = 0; i < expectedReviews.Count; i++)
            {
                Assert.AreEqual(expectedReviews[i].Id, actualReviews[i].Id);
                Assert.AreEqual(expectedReviews[i].UserId, actualReviews[i].UserId);
                Assert.AreEqual(expectedReviews[i].RestaurantId, actualReviews[i].RestaurantId);
                Assert.AreEqual(expectedReviews[i].Title, actualReviews[i].Title);
                Assert.AreEqual(expectedReviews[i].UserReview, actualReviews[i].UserReview);
                Assert.AreEqual(expectedReviews[i].PriceRatingId, actualReviews[i].PriceRatingId);
                Assert.AreEqual(expectedReviews[i].StarRatingId, actualReviews[i].StarRatingId);
                Assert.AreEqual(expectedReviews[i].IsDeleted, actualReviews[i].IsDeleted);
            }
        }

        [TestMethod]
        public async Task CanGetActiveReviewsByRestaurantNameAndCityAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedReviews = new List<ReviewDto>()
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
                }
            };

            // Act
            var reviews = await controller.GetActiveReviewsByRestaurantNameAndCityAsync("McDonald's", "PittsBurgh");
            var actualReviews = reviews.ToList();

            // Assert
            Assert.IsNotNull(actualReviews);
            Assert.AreEqual(expectedReviews.Count, actualReviews.Count());
            for (int i = 0; i < expectedReviews.Count; i++)
            {
                Assert.AreEqual(expectedReviews[i].Id, actualReviews[i].Id);
                Assert.AreEqual(expectedReviews[i].UserId, actualReviews[i].UserId);
                Assert.AreEqual(expectedReviews[i].RestaurantId, actualReviews[i].RestaurantId);
                Assert.AreEqual(expectedReviews[i].Title, actualReviews[i].Title);
                Assert.AreEqual(expectedReviews[i].UserReview, actualReviews[i].UserReview);
                Assert.AreEqual(expectedReviews[i].PriceRatingId, actualReviews[i].PriceRatingId);
                Assert.AreEqual(expectedReviews[i].StarRatingId, actualReviews[i].StarRatingId);
                Assert.AreEqual(expectedReviews[i].IsDeleted, actualReviews[i].IsDeleted);
            }
        }

        [TestMethod]
        public async Task CanAddReviewAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedReview = new ReviewDto
            {
                Id = 4,
                UserId = 1,
                RestaurantId = 1,
                Title = "McDonalds is yucky.",
                UserReview = "bad, bad, bad",
                PriceRatingId = 1,
                StarRatingId = 1,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOn = null
            };

            // Act
            var actualReview = await controller.AddReviewAsync(expectedReview);

            // Assert
            Assert.IsNotNull(actualReview);

            Assert.AreEqual(expectedReview.Id, actualReview.Id);
            Assert.AreEqual(expectedReview.UserId, actualReview.UserId);
            Assert.AreEqual(expectedReview.RestaurantId, actualReview.RestaurantId);
            Assert.AreEqual(expectedReview.Title, actualReview.Title);
            Assert.AreEqual(expectedReview.UserReview, actualReview.UserReview);
            Assert.AreEqual(expectedReview.PriceRatingId, actualReview.PriceRatingId);
            Assert.AreEqual(expectedReview.StarRatingId, actualReview.StarRatingId);
            Assert.AreEqual(expectedReview.IsDeleted, actualReview.IsDeleted);
        }

        [TestMethod]
        public async Task CanAddRestaurantAndReviewAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedRestaurant = new AddRestaurantDto
            {
                Name = "New Restaurant",
                Description = "stuff.",
                Address1 = "111 Road",
                Address2 = null,
                City = "Pittsburgh",
                State = "PA",
                PostalCode = "15239",
                IsDeleted = false
            };

            var expectedReview = new AddReviewDto
            {
                UserId = 1,
                // Restaurant Id will be auto generated
                Title = "Not a fan.",
                UserReview = "New restaurant is expensive and gross.",
                PriceRatingId = 4,
                StarRatingId = 1,
                IsDeleted = false
            };

            var expectedRestaurantAndReview = new AddRestaurantAndReviewDto();
            expectedRestaurantAndReview.Restaurant = expectedRestaurant;
            expectedRestaurantAndReview.Review = expectedReview;

            // Act
            var actualRestaurantAndReview = await controller.AddRestaurantAndReviewAsync(expectedRestaurantAndReview);

            // Assert
            Assert.IsNotNull(actualRestaurantAndReview);

            Assert.AreEqual(actualRestaurantAndReview.Restaurant.Id, TestData.RestaurantList.Count);
            Assert.AreEqual(actualRestaurantAndReview.Review.Id, TestData.ReviewList.Count);
            Assert.AreEqual(actualRestaurantAndReview.Review.RestaurantId, actualRestaurantAndReview.Restaurant.Id);
        }
    }
}
