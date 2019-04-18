using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using static Auction.Entities.Enums.CommonEnum;
using Auction.Entities;
using Auction.Identity;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auction.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>,
                                                        ApplicationUserRole, IdentityUserLogin<Guid>,
                                                        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("Data Source=blogging.db");
            // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        // /// <summary>
        // /// 用户
        // /// </summary>
        // public DbSet<User> Users { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public DbSet<Photo> Photos { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        #region DbQuery
        /// <summary>
        /// 
        /// </summary>
        // public DbQuery<PermissionWithAssignProperty> PermissionWithAssignProperty { get; set; }
        #endregion

        /// <summary>
        /// 数据注释[DatabaseGenerated(DatabaseGeneratedOption.None)]
        /// DatabaseGeneratedOption 可选项 与 Fluent API的对应关系
        /// Computed：ValueGeneratedOnAddOrUpdate
        /// Identity：ValueGeneratedOnAdd
        /// None：ValueGeneratedNever
        /// 数据注释中不能设定数据库中的默认值，只能在Fluent API中指定
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("newid()");
                entity.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.LastUpdatedAt)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.CreatedAt)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.LastUpdatedAt)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("getdate()");
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.HasMany<ApplicationUserRole>()
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            builder.Entity<ApplicationUser>().ToTable("st_users");
            builder.Entity<ApplicationRole>().ToTable("st_roles");
            builder.Entity<ApplicationUserRole>().ToTable("st_user_roles");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("st_role_claims");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("st_user_claims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("st_user_logins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("st_user_tokens");

            builder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(IsDeleted.No);
                entity.Property(e => e.CreatedAt)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsHiddenAfterSold)
                    .HasDefaultValue(false);
                entity.Property(e => e.LastUpdatedAt)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("getdate()");
            });

            builder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.Id)
                     .ValueGeneratedNever()
                     .HasDefaultValueSql("newid()");
                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(IsDeleted.No);
                entity.Property(e => e.IsSold)
                     .HasDefaultValue(IsSold.No);
                entity.Property(e => e.IsPurchase)
                    .HasDefaultValue(IsPurchase.No);
                entity.Property(e => e.CreatedAt)
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("getdate()");
                entity.Property(e => e.LastUpdatedAt)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("getdate()");

                entity.HasOne(e => e.CoverPhoto)
                    .WithOne(p => p.CoverEquipment);
                entity.HasMany(e => e.Photos)
                    .WithOne(p => p.Equipment);
                entity.HasMany(e => e.ExteriorPhotos)
                    .WithOne(p => p.ExteriorEquipment);
                entity.HasMany(e => e.TrackedChassisPhotos)
                    .WithOne(p => p.TrackedChassisEquipment);
                entity.HasMany(e => e.CabPhotos)
                    .WithOne(p => p.CabEquipment);
                entity.HasMany(e => e.BoomPhotos)
                    .WithOne(p => p.BoomEquipment);
                entity.HasMany(e => e.EnginePhotos)
                    .WithOne(p => p.EngineEquipment);
            });
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override EntityEntry Update(object entity)
        {
            return base.Update(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            return base.Update<TEntity>(entity);
        }
    }
}
