﻿// <auto-generated />
using System;
using Gaver.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gaver.Web.Migrations
{
    [DbContext(typeof(GaverContext))]
    [Migration("20211212133831_Remove_invitation_accepted")]
    partial class Remove_invitation_accepted
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Gaver.Data.Entities.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("WishListId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WishListId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("Gaver.Data.Entities.InvitationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<Guid>("Token")
                        .HasColumnType("uuid");

                    b.Property<int>("WishListId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WishListId");

                    b.ToTable("InvitationTokens");
                });

            modelBuilder.Entity("Gaver.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PrimaryIdentityId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryIdentityId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserFriendConnection", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("FriendId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("UserFriendConnections");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserGroupConnection", b =>
                {
                    b.Property<int>("UserGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("UserGroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroupConnections");
                });

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BoughtByUserId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Url")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("WishListId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BoughtByUserId");

                    b.HasIndex("WishListId");

                    b.ToTable("Wishes");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("WishesOrder")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("WishLists");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Url")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("WishId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WishId");

                    b.ToTable("WishOptions");
                });

            modelBuilder.Entity("Gaver.Data.Entities.ChatMessage", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gaver.Data.Entities.WishList", "WishList")
                        .WithMany("ChatMessages")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Gaver.Data.Entities.InvitationToken", b =>
                {
                    b.HasOne("Gaver.Data.Entities.WishList", "WishList")
                        .WithMany("InvitationTokens")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserFriendConnection", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gaver.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserGroup", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserGroupConnection", b =>
                {
                    b.HasOne("Gaver.Data.Entities.UserGroup", "UserGroup")
                        .WithMany("UserGroupConnections")
                        .HasForeignKey("UserGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gaver.Data.Entities.User", "User")
                        .WithMany("UserGroupConnections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "BoughtByUser")
                        .WithMany("BoughtWishes")
                        .HasForeignKey("BoughtByUserId");

                    b.HasOne("Gaver.Data.Entities.WishList", "WishList")
                        .WithMany("Wishes")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoughtByUser");

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
                {
                    b.HasOne("Gaver.Data.Entities.User", "User")
                        .WithOne("WishList")
                        .HasForeignKey("Gaver.Data.Entities.WishList", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishOption", b =>
                {
                    b.HasOne("Gaver.Data.Entities.Wish", "Wish")
                        .WithMany("Options")
                        .HasForeignKey("WishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wish");
                });

            modelBuilder.Entity("Gaver.Data.Entities.User", b =>
                {
                    b.Navigation("BoughtWishes");

                    b.Navigation("UserGroupConnections");

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Gaver.Data.Entities.UserGroup", b =>
                {
                    b.Navigation("UserGroupConnections");
                });

            modelBuilder.Entity("Gaver.Data.Entities.Wish", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("Gaver.Data.Entities.WishList", b =>
                {
                    b.Navigation("ChatMessages");

                    b.Navigation("InvitationTokens");

                    b.Navigation("Wishes");
                });
#pragma warning restore 612, 618
        }
    }
}
