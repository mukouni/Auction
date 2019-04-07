﻿// <auto-generated />
using System;
using Auction.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Auction.Migrations
{
    [DbContext(typeof(AuctionDbContext))]
    [Migration("20190328215414_addEquipmentRemarkAndDistance")]
    partial class addEquipmentRemarkAndDistance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Auction.Entities.Equipment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("AuctionHouse")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<Guid?>("CreatedByUserGuid");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("DealPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal?>("DealPriceRMB")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<long?>("Height");

                    b.Property<int?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int?>("IsPurchase")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int?>("IsSold")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.Property<long?>("Long");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ModifiedByUserGuid");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ProductionDate");

                    b.Property<string>("RBCode")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Remark")
                        .HasMaxLength(5000);

                    b.Property<DateTime?>("SoldAt");

                    b.Property<decimal?>("Volume")
                        .HasColumnType("decimal(18, 3)");

                    b.Property<double?>("Weight");

                    b.Property<long?>("Width");

                    b.Property<long?>("WorkingDistance");

                    b.Property<string>("WorkingDistanceUnit")
                        .HasColumnType("nvarchar(20)");

                    b.Property<long?>("WorkingTime");

                    b.HasKey("Id");

                    b.ToTable("ac_equipment");
                });

            modelBuilder.Entity("Auction.Entities.LoginLogging", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<Guid?>("CreatedByUserGuid");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ip");

                    b.Property<int?>("IsDeleted");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid?>("ModifiedByUserGuid");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Platform");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("st_login_log");
                });

            modelBuilder.Entity("Auction.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<Guid?>("CreatedByUserGuid");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("EquipmentId");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<long?>("FileSize");

                    b.Property<bool?>("IsCover")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<int?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.Property<Guid?>("ModifiedByUserGuid");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OriginName")
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("Ranking");

                    b.Property<string>("RequestPath")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SavePath")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.ToTable("ac_photo");
                });

            modelBuilder.Entity("Auction.Identity.Entities.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("st_roles");
                });

            modelBuilder.Entity("Auction.Identity.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AvatorPath")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("DeadlineAt");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(800)");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("IsDeleted");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RealName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("st_users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("st_role_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("st_user_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("st_user_logins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("st_user_roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("st_user_tokens");
                });

            modelBuilder.Entity("Auction.Entities.LoginLogging", b =>
                {
                    b.HasOne("Auction.Identity.Entities.ApplicationUser", "User")
                        .WithMany("LoginLogging")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Auction.Entities.Photo", b =>
                {
                    b.HasOne("Auction.Entities.Equipment", "Equipment")
                        .WithMany("Photos")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Auction.Identity.Entities.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Auction.Identity.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Auction.Identity.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Auction.Identity.Entities.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Auction.Identity.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Auction.Identity.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}