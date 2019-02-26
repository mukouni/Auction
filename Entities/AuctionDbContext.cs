using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDbContext : DbContext
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

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public DbSet<Photo> Photos { get; set; }



        /// <summary>
        /// 设备
        /// </summary>
        public DbSet<Equipment> Equipments { get; set; }

        /// <summary>
        /// 设备图片对应
        /// </summary>
        public DbSet<EquipmentPhoto> EquipmentPhotos { get; set; }

        #region DbQuery
        /// <summary>
        /// 
        /// </summary>
        // public DbQuery<PermissionWithAssignProperty> PermissionWithAssignProperty { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(IsDeleted.No);
                entity.Property(e => e.IsLocked)
                    .HasDefaultValue(IsLocked.UnLocked);
                entity.Property(e => e.CreatedOn)
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.UserRole)
                    .HasDefaultValue(UserRole.Guest);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(IsDeleted.No);
                entity.Property(e => e.CreatedOn)
                    .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Equipment>(entity =>
           {
               entity.Property(e => e.IsDelete)
                   .HasDefaultValue(IsDeleted.No);
               entity.Property(e => e.IsSold)
                    .HasDefaultValue(IsSold.No);
               entity.Property(e => e.CreatedOn)
                   .HasDefaultValueSql("getdate()");
           });

            modelBuilder.Entity<EquipmentPhoto>(entity =>
            {
                entity.HasKey(x => new
                {
                    x.EquipmentGuid,
                    x.PhotoGuid
                });

                // entity.HasOne(x => x.Equipment)
                //     .WithMany(x => x.EquipmentPhotos)
                //     .HasForeignKey(x => x.EquipmentGuid)
                //     .OnDelete(DeleteBehavior.Restrict);

                // entity.HasOne(x => x.Photo)
                //     .WithMany(x => x.EquipmentPhotos)
                //     .HasForeignKey(x => x.PhotoGuid)
                //     .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }


        // public override EntityEntry<User> Update<User>(User entity){
        //     return (EntityEntry)entity;
        // }
    }
}
