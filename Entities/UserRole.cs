using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        // public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        // public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}