# RestaurantReviews.API

## Setup
I decided to implement the API with Visual Studio 2022 Community.  It is .NET version 6.0 and should run on all platforms that are supported.  The database is Entity Framework Core code-first and uses a Postgres database.  

All of the Nuget packages should be supported.  This includes packages for Entity Framework, Npgsql provider for Entity Framework, MS Test & Moq for Unit Testing, and Swashbuckle/Swagger for API exposure and documentation.  Please make sure to update the packages by building the solution or by selecting Restore Package from various areas in the VS user interface or power shell command line.

To Set-up the Database:

1.  In Visual Studio, under the RestaurantReviews.API project, edit the file appsettings.json
2.  Change the ConnectionStrings to be applicable for your setup.  Here is what mine looks like:

"ConnectionStrings": {
    "RestaurantReviewsDb": "Server=localhost;Database=RestaurantReviewsDb;Port=5432;User Id=postgres;Password=Postgres123"
  }

3.  Open a Package Manager Console under View -> Other Windows
4.  Make sure the dropdown for "Default project:" is set to RestaurantReviews.Data
5.  Issue the command "Update-Database" in the Package Manager Console.  (if that fails, issue the command:  "EntityFrameworkCore\Update-Database")


API Documentation through Swagger and OpenAPI Documentation:

1.  The documentation should be automatically built and stored in the RestaurantReviews.API/bin/Debug/net6.0/RestaurantReviews.API.xml file.
2.  The option to turn this off/on is in the RestaurantReviews.API project Build -> Output settings under "Documentation File".  Please ensure that it is turned on (checked).  If not, there will be an error on startup in Program.cs where the AddSwaggerGen is set up because it is looking for a specific XML file.

## Documentation for API
Because the Swagger OpenApiInfo is set up, the comments that decorate each Contoller and each Controller method will show up in the Swagger page upon debugging the API.  The comments are intended to provide enough information for a developer working on the front-end to be able to use the API effectively.  

## Design
I decided to use Data Access Objects (DAO) to add a layer of separation between the Data and the API.  Even though DAO is an older concept, it still works pretty well in this example.  The data transferred between the DAO and API are very simple DTOs (Data Transfer Objects).  The DAO layer is very basic and involves the data access through Entity Framework and linq calls.  If more complex business logic is needed, a service layer could be provided as another abstraction that would be added between the DAO and API.  However, since the business logic in this example is basic, I decided to only provide the one level of abstraction.  The DTOs are also very simple which include getters and setters, but can be used to pass distinct data if needed.  For example, I used a DTO called RestaurantAndReviewDto to be used when a user creates a review for a restaurant that is not already in the system.  It is used to create both a new Restaurant and a new Review.

The database design is also very basic.  I used code-first Entity Framework Core because it works very nicely with Postgres and should be supported on many platforms.  The major tables are User, Restaurant and Review.  The data-types are likewise as simple as possible.

Also, for an extra twist, I added a PriceRating and StarRating table to the database.  This table stores the dollar sign ratings (PriceRating) and the number of stars (StarRating).  The Review table has PriceRatingId and StarRatingId which point to the Ids from these tables.  Every time a review is saved, the Restaurant's AveragePriceRating and AverageStarRating will be re-calculated in the method ReviewDao.cs -> CalculateAndUpdateRestaurantRatingsAsync.

## Major Functionality

1. Create a user
- This was implemented in the UsersController -> AddUserAsync

2. Create a restaurant
- This was implemented in the RestaurantsController -> AddRestaurantAsync

3. Create a review for a restaurant
- This was implemented in the ReviewsController -> AddReviewAsync and also in AddRestaurantAndReviewAsync

4. Get of a list of reviews by user
- This was implemented in ReviewsController -> GetActiveReviewsByUserAsync

5. Get of a list of reviews by restaurant
- This was implemented in ReviewsController -> GetActiveReviewsByRestaurantAsync

6. Get a list of restaurants by city
- This was implemented in RestaurantsController -> GetActiveRestaurantsByCityAsync

7. Delete a review
- This was implemented in ReviewsController -> DeleteReviewAsync

8. Block a user from posting a review
- This was implemented by setting the user as blocked in UsersController -> BlockUserAsync, and to prevent a review from going through if a user was blocked was implemented in UserDao.cs -> IsUserBlockedOrDeletedAsync

## Extra Functionality -- Additional twists and ideas I implemented just for fun.

1. Authenticate user
2. Unblock a user
3. Delete a user
4. Undelete a user
5. Delete a restaurant
6. Deleting a restaurant deletes all associated reviews
7. Add price rating and star rating to a review
8. Every new review will update the restaurant's average price rating and star rating

# Things To Consider
My answers to the things to consider:

* We are building a mobile application independent of your development, what might be the best way to communicate to other developer how to user your API?
- I decided to use Swagger to add some basic API documentation.  In the documentation, I tried to specify any restrictions or errors that might be thrown because of issues.  Also, Swagger does a very nice job of showing the endpoints, allowing to "Try it out" and documenting the DTOs.

* After you turn your code over for the API, how might you help ensure future developers can feel confident updating it?
- I added the DAO layer to allow for the API layer to be as clean as possible.  Developers should feel confident because they can simply add/update/remove any functionality by following the same pattern.  If backwards compatibility support is needed, Developers will need to be mindful of Database structural changes and functional changes.  The deletions are not permanent, if something is deleted, the IsDeleted flag is set to true.  This should help the data remain consistant because nothing will be lost forever.  It also adds some audit capabilities if needed.  I also added a small set of unit tests for the controllers that implement the major DAO functionality so that if breaking changes are made, the unit tests will fail.

## Thank you!
I appreciate the time you take to review my project!  I know how time consuming a large code review can be.  It has been very enjoyable to work on.  Thank you very much for the opportunity.  I hope that my solution is satisfactory, and I would love to hear any positive or negative feedback so that I may improve my coding skills.
