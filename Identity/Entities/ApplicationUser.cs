using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Auction.Entities;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Identity.Entities
{
    /// <summary>
    /// 登陆用户
    /// </summary>
    // [Table("st_user")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        // /// <summary>
        // /// 登陆名
        // /// </summary>
        // [Column(TypeName = "nvarchar(50)")]
        // public string LoginName { get; set; }

        // /// <summary>
        // /// 邮箱
        // /// </summary>
        // [Column(TypeName = "nvarchar(50)")]
        // public string Email { get; set; }

        // /// <summary>
        // /// 手机号
        // /// </summary>
        // [Column(TypeName = "nvarchar(20)")]
        // public string Phone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string RealName { get; set; }

        // /// <summary>
        // /// 密码
        // /// </summary>
        // [Column(TypeName = "nvarchar(255)")]
        // public string Password { get; set; }

        // /// <summary>
        // /// 账户是否锁定
        // /// </summary>
        // public IsLocked? IsLocked { get; set; }

        /// <summary>
        /// 用户描述信息
        /// </summary>
        [Column(TypeName = "nvarchar(800)")]
        public string Description { get; set; }

        // /// <summary>
        // /// 头像 path+name+后缀
        // /// </summary>
        // public string LastLoginIp { get; set; }

        // /// <summary>
        // /// 头像 path+name+后缀
        // /// </summary>
        // public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 头像 path+name+后缀
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string AvatorPath { get; set; }


        /// <summary>
        /// 会员到期日期
        /// </summary>
        public DateTime? DeadlineAt { get; set; }

        [Column(Order = 101)]
        public IsDeleted? IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Order = 102)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// 最近修改时间
        /// EF.Property<DateTime>(b, "LastUpdated")
        /// context.Entry(myBlog).Property("LastUpdated").CurrentValue
        /// EF Code Model 中定义的shadow(隐藏、卷影，直译阴影)属性LastUpdated和这个字段是同一个作用。两者不确定相同，但是可以肯定时间相近
        /// </summary>
        [Column(Order = 103)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedAt { get; set; }


        /// <summary>
        /// 用户的角色中间表
        /// </summary>
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        // /// <summary>
        // /// 用户的角色
        // /// </summary>
        // [NotMapped]
        // public ICollection<ApplicationRole> Roles { get; set; }
    }
}
