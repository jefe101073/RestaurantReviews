using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RestaurantReviews.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: false),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    PriceRatingId = table.Column<int>(type: "integer", nullable: false),
                    StarRatingId = table.Column<int>(type: "integer", nullable: false),
                    DeletedByUserId = table.Column<int>(type: "integer", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RestaurantId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UserReview = table.Column<string>(type: "text", nullable: false),
                    PriceRatingId = table.Column<int>(type: "integer", nullable: false),
                    StarRatingId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedByUserId = table.Column<int>(type: "integer", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StarRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    IsUserBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedByUserId = table.Column<int>(type: "integer", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PriceRatings",
                columns: new[] { "Id", "Text", "Value" },
                values: new object[,]
                {
                    { 1, "$", "Inexpensive, usually $10 and under" },
                    { 2, "$$", "Moderately expensive, usually between $10-$25" },
                    { 3, "$$$", "Expensive, usually between $25-$45" },
                    { 4, "$$$$", "Very Expensive, usually $50 and up" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address1", "Address2", "City", "DeletedByUserId", "DeletedOn", "Description", "IsDeleted", "Name", "PostalCode", "PriceRatingId", "StarRatingId", "State" },
                values: new object[,]
                {
                    { 1, "46 18th Street", null, "Pittsburgh", null, null, "Sandwich Shop", false, "Primanti Brothers", "15222", 2, 4, "PA" },
                    { 2, "1279 Camp Horne Road", null, "Pittsburgh", null, null, "Pizza and Wings", false, "Pizza Hut", "15237", 1, 3, "PA" },
                    { 3, "634 Camp Horne Road", null, "Pittsburgh", null, null, "Upscale-casual eatery featuring modern American dishes including crab cakes, beef tenderloin & veal.", false, "Willow", "15237", 3, 5, "PA" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "DeletedByUserId", "DeletedOn", "IsDeleted", "PriceRatingId", "RestaurantId", "StarRatingId", "Title", "UserId", "UserReview" },
                values: new object[,]
                {
                    { 1, null, null, false, 2, 1, 4, "A Real Pittsburgh Tradition", 1, "Best sandwiches with cole slaw and fries in the world." },
                    { 2, null, null, false, 1, 2, 3, "No wonder Pizza Hut has so many locations!", 1, "Consistant quality and very affordable." },
                    { 3, null, null, false, 3, 3, 5, "I never knew steak could taste so good!", 1, "Wonderful service, the food is top-notch, would recommend for anyone who needs a special occasion." }
                });

            migrationBuilder.InsertData(
                table: "StarRatings",
                columns: new[] { "Id", "Text", "Value" },
                values: new object[,]
                {
                    { 1, "*", "Poor" },
                    { 2, "**", "Moderate" },
                    { 3, "***", "Good" },
                    { 4, "****", "Very Good" },
                    { 5, "*****", "Excellent" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedByUserId", "DeletedOn", "Email", "FirstName", "IsDeleted", "IsUserBlocked", "LastName", "Password" },
                values: new object[,]
                {
                    { 1, null, null, "admin@admin.com", "Admin", false, false, "User", "hZZDS7+9BmZMQxPCT5trrO/VKbSo5+34w/c/U1BZags=" },
                    { 2, null, null, "jefe101073@gmail.com", "Jeff", false, false, "McCann", "hZZDS7+9BmZMQxPCT5trrO/VKbSo5+34w/c/U1BZags=" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceRatings");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "StarRatings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
