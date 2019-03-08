using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Identity.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        // public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        // public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}