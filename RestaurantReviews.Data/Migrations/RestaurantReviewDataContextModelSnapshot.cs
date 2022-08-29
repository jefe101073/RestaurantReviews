﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RestaurantReviews.Data;

#nullable disable

namespace RestaurantReviews.Data.Migrations
{
    [DbContext(typeof(RestaurantReviewDataContext))]
    partial class RestaurantReviewDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("RestaurantReviews.Data.PriceRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PriceRatings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Text = "$",
                            Value = "Inexpensive, usually $10 and under"
                        },
                        new
                        {
                            Id = 2,
                            Text = "$$",
                            Value = "Moderately expensive, usually between $10-$25"
                        },
                        new
                        {
                            Id = 3,
                            Text = "$$$",
                            Value = "Expensive, usually between $25-$45"
                        },
                        new
                        {
                            Id = 4,
                            Text = "$$$$",
                            Value = "Very Expensive, usually $50 and up"
                        });
                });

            modelBuilder.Entity("RestaurantReviews.Data.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Address2")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("DeletedByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PriceRatingId")
                        .HasColumnType("integer");

                    b.Property<int?>("StarRatingId")
                        .HasColumnType("integer");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Restaurants");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address1 = "46 18th Street",
                            City = "Pittsburgh",
                            Description = "Sandwich Shop",
                            IsDeleted = false,
                            Name = "Primanti Brothers",
                            PostalCode = "15222",
                            PriceRatingId = 2,
                            StarRatingId = 4,
                            State = "PA"
                        },
                        new
                        {
                            Id = 2,
                            Address1 = "1279 Camp Horne Road",
                            City = "Pittsburgh",
                            Description = "Pizza and Wings",
                            IsDeleted = false,
                            Name = "Pizza Hut",
                            PostalCode = "15237",
                            PriceRatingId = 1,
                            StarRatingId = 3,
                            State = "PA"
                        },
                        new
                        {
                            Id = 3,
                            Address1 = "634 Camp Horne Road",
                            City = "Pittsburgh",
                            Description = "Upscale-casual eatery featuring modern American dishes including crab cakes, beef tenderloin & veal.",
                            IsDeleted = false,
                            Name = "Willow",
                            PostalCode = "15237",
                            PriceRatingId = 3,
                            StarRatingId = 5,
                            State = "PA"
                        });
                });

            modelBuilder.Entity("RestaurantReviews.Data.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<int?>("DeletedByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("PriceRatingId")
                        .HasColumnType("integer");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("integer");

                    b.Property<int>("StarRatingId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("UserReview")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            PriceRatingId = 2,
                            RestaurantId = 1,
                            StarRatingId = 4,
                            Title = "A Real Pittsburgh Tradition",
                            UserId = 1,
                            UserReview = "Best sandwiches with cole slaw and fries in the world."
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            PriceRatingId = 1,
                            RestaurantId = 2,
                            StarRatingId = 3,
                            Title = "No wonder Pizza Hut has so many locations!",
                            UserId = 1,
                            UserReview = "Consistant quality and very affordable."
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            PriceRatingId = 3,
                            RestaurantId = 3,
                            StarRatingId = 5,
                            Title = "I never knew steak could taste so good!",
                            UserId = 1,
                            UserReview = "Wonderful service, the food is top-notch, would recommend for anyone who needs a special occasion."
                        });
                });

            modelBuilder.Entity("RestaurantReviews.Data.StarRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("StarRatings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Text = "*",
                            Value = "Poor"
                        },
                        new
                        {
                            Id = 2,
                            Text = "**",
                            Value = "Moderate"
                        },
                        new
                        {
                            Id = 3,
                            Text = "***",
                            Value = "Good"
                        },
                        new
                        {
                            Id = 4,
                            Text = "****",
                            Value = "Very Good"
                        },
                        new
                        {
                            Id = 5,
                            Text = "*****",
                            Value = "Excellent"
                        });
                });

            modelBuilder.Entity("RestaurantReviews.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<int?>("DeletedByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUserBlocked")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@admin.com",
                            FirstName = "Admin",
                            IsDeleted = false,
                            IsUserBlocked = false,
                            LastName = "User",
                            Password = "hZZDS7+9BmZMQxPCT5trrO/VKbSo5+34w/c/U1BZags="
                        },
                        new
                        {
                            Id = 2,
                            Email = "jefe101073@gmail.com",
                            FirstName = "Jeff",
                            IsDeleted = false,
                            IsUserBlocked = false,
                            LastName = "McCann",
                            Password = "hZZDS7+9BmZMQxPCT5trrO/VKbSo5+34w/c/U1BZags="
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
