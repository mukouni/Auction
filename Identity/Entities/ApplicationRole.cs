using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Identity.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public const string Admin = "Admin";

        public const string Developer = "Developer";

        public const string Guest = "Guest";

        public const string Staff = "Staff";

        public const string Member = "Member";

        public const string Other = "Other";

        // public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        // public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}