using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using static Auction.Entities.Enums.CommonEnum;
using Auction.Identity;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auction.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDbContext : AppIdentityDbContext
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

        public DbSet<LoginLogging> LoginLoggings { get; set; }

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
            // builder.Entity<ApplicationUser>().ToTable("st_users");
            // builder.Entity<ApplicationRole>().ToTable("st_roles");
            // builder.Entity<IdentityUserRole<Guid>>().ToTable("st_user_roles");
            // builder.Entity<IdentityRoleClaim<Guid>>().ToTable("st_role_claims");
            // builder.Entity<IdentityUserClaim<Guid>>().ToTable("st_user_claims");
            // builder.Entity<IdentityUserLogin<Guid>>().ToTable("st_user_logins");
            // builder.Entity<IdentityUserToken<Guid>>().ToTable("st_user_tokens");
            // modelBuilder.Entity<User>(entity =>
            // {
            //     entity.Property(e => e.Guid)
            //         .ValueGeneratedNever()
            //         .HasDefaultValueSql("newid()");
            //     entity.Property(e => e.IsDelete)
            //         .HasDefaultValue(IsDeleted.No);
            //     entity.Property(e => e.IsLocked)
            //         .HasDefaultValue(IsLocked.UnLocked);
            //     entity.Property(e => e.Role)
            //         .HasDefaultValue(UserRole.Guest);
            //     entity.Property(e => e.CreatedAt)
            //         .ValueGeneratedOnAdd()
            //         .HasDefaultValueSql("getdate()");
            //     entity.Property(e => e.LastUpdatedAt)
            //         .ValueGeneratedOnAddOrUpdate()
            //         .HasDefaultValueSql("getdate()");
            // });

            builder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasDefaultValueSql("newid()");
                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(IsDeleted.No);
                entity.Property(e => e.IsHome)
                    .HasDefaultValue(false);
                entity.Property(e => e.CreatedAt)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.LastUpdatedAt)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("getdate()");
            });

            builder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.Id)
                     .ValueGeneratedNever()
                     .HasDefaultValueSql("newid()");
                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(IsDeleted.No);
                entity.Property(e => e.IsSold)
                     .HasDefaultValue(IsSold.No);
                entity.Property(e => e.CreatedAt)
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("getdate()");
                entity.Property(e => e.LastUpdatedAt)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("getdate()");
            });

            base.OnModelCreating(builder);
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
