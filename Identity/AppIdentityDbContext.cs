using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Auction.Identity.Entities;

namespace Auction.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("st_users");
            builder.Entity<ApplicationRole>().ToTable("st_roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("st_user_roles");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("st_role_claims");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("st_user_claims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("st_user_logins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("st_user_tokens");
        }
    }
}
