using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Gaver.Data;

namespace Gaver.Web.Migrations
{
    [DbContext(typeof(GaverContext))]
    partial class GaverContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Gaver.Data.Entities.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Sqlite:DefaultValueSql", "CURRENT_TIMESTAMP");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("UserId");

                    b.Property<int>("WishListId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WishListId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("Gaver.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 40);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BoughtByUserId");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 4000);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Url")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("WishListId");

                    b.HasKey("Id");

                    b.HasIndex("BoughtByUserId");

                    b.HasIndex("WishListId");

                    b.ToTable("Wishes");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WishLists");
                });

            modelBuilder.Entity("Gaver.Data.Entities.ChatMessage", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Gaver.Data.Entities.WishList", "WishList")
                        .WithMany("ChatMessages")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "BoughtByUser")
                        .WithMany("BoughtWishes")
                        .HasForeignKey("BoughtByUserId");

                    b.HasOne("Gaver.Data.Entities.WishList", "WishList")
                        .WithMany("Wishes")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "User")
                        .WithMany("WishLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
