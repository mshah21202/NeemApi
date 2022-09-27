﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeemApi.Data;

#nullable disable

namespace NeemApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220919094326_AddedDescriptionToProducts")]
    partial class AddedDescriptionToProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("NeemApi.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NeemApi.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NeemApi.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PublicId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("NeemApi.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<float>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("NeemApi.Entities.UserFavorite", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFavorite");
                });

            modelBuilder.Entity("NeemApi.Entities.Photo", b =>
                {
                    b.HasOne("NeemApi.Entities.Product", "Product")
                        .WithMany("Photos")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("NeemApi.Entities.Product", b =>
                {
                    b.HasOne("NeemApi.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("NeemApi.Entities.UserFavorite", b =>
                {
                    b.HasOne("NeemApi.Entities.Product", "Product")
                        .WithMany("UserFavorite")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeemApi.Entities.AppUser", "User")
                        .WithMany("UserFavorite")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeemApi.Entities.AppUser", b =>
                {
                    b.Navigation("UserFavorite");
                });

            modelBuilder.Entity("NeemApi.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("NeemApi.Entities.Product", b =>
                {
                    b.Navigation("Photos");

                    b.Navigation("UserFavorite");
                });
#pragma warning restore 612, 618
        }
    }
}
