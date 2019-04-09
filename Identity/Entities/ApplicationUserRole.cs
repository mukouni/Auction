using System;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auction.Entities
{
    [Table("st_user_roles")]
    public partial class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}