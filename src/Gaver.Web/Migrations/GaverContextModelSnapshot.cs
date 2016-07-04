using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Gaver.Data;

namespace Gaver.Web.Migrations
{
    [DbContext(typeof(GaverContext))]
    partial class GaverContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<int?>("WishListId");

                    b.HasKey("Id");

                    b.HasIndex("WishListId");

                    b.ToTable("Wishes");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("WishLists");
                });

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.HasOne("Gaver.Data.Entities.WishList")
                        .WithMany("Wishes")
                        .HasForeignKey("WishListId");
                });
        }
    }
}
