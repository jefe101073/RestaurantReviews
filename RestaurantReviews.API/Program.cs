using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Dao;
using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Entity Framework - Add DbContext to services - Setup Connection string from appsettings.json "RestaurantReviewsDb"
builder.Services.AddDbContext<RestaurantReviewDataContext>(
    z => z.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantReviewsDb")));

// Default Swagger setup for basic API Documentation
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRestaurantDao, RestaurantDao>();
builder.Services.AddScoped<IReviewDao, ReviewDao>();
builder.Services.AddScoped<IUserDao, UserDao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
