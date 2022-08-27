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


## Functionality
There are 3 controllers 