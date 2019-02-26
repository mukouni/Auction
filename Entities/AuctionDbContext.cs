using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public DbSet<Photo> Photo { get; set; }



        /// <summary>
        /// 设备
        /// </summary>
        public DbSet<Photo> Equipment { get; set; }

        /// <summary>
        /// 设备图片对应
        /// </summary>
        public DbSet<Equipment> EquipmentPhoto { get; set; }

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

            // modelBuilder.Entity<User>(entity =>
            // {
            //     entity.HasIndex(x => x.Guid).IsUnique();
            // });

            // modelBuilder.Entity<Photo>(entity =>
            // {
            //     entity.HasIndex(x => x.Guid).IsUnique();
            // });

            //  modelBuilder.Entity<Equipment>(entity =>
            // {
            //     entity.HasIndex(x => x.Guid).IsUnique();
            // });

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
    }
}
