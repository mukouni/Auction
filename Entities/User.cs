using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    /// <summary>
    /// 登陆用户
    /// </summary>
    [Table("ac_user")]
    public class User : BaseEntity
    {

        [Required]
        [Column(TypeName = "nvarchar(50)", Order = 10)]
        public string LoginName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public Photo Avator { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string RealName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Password { get; set; }

        public IsLocked IsLocked { get; set; } = IsLocked.UnLocked;

        /// <summary>
        /// 用户描述信息
        /// </summary>
        [Column(TypeName = "nvarchar(800)")]
        public string Description { get; set; }

        /// <summary>
        /// 用户的角色
        /// </summary>
        public virtual UserRole UserRole { get; set; } = UserRole.Guest;

    }

}
