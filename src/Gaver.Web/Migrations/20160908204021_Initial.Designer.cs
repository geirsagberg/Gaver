using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Gaver.Data;

namespace Gaver.Web.Migrations
{
    [DbContext(typeof(GaverContext))]
    [Migration("20160908204021_Initial")]
    partial class Initial
    {
	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
	    modelBuilder
		.HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

	    modelBuilder.Entity("Gaver.Data.Entities.User", b =>
		{
		    b.Property<int>("Id")
			.ValueGeneratedOnAdd();

		    b.Property<string>("Name")
			.IsRequired()
			.HasAnnotation("MaxLength", 40);

		    b.HasKey("Id");

		    b.ToTable("Users");
		});

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

		    b.Property<int?>("UserId");

		    b.HasKey("Id");

		    b.HasIndex("UserId");

		    b.ToTable("WishLists");
		});

	    modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
		{
		    b.HasOne("Gaver.Data.Entities.WishList")
			.WithMany("Wishes")
			.HasForeignKey("WishListId");
		});

	    modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
		{
		    b.HasOne("Gaver.Data.Entities.User")
			.WithMany("WishLists")
			.HasForeignKey("UserId");
		});
	}
    }
}
