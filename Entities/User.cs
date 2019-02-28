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
    [Table("st_user")]
    public class User : BaseEntity
    {
        /// <summary>
        /// 登陆名
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string LoginName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(TypeName = "nvarchar(20)")]
        public string Phone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string RealName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string Password { get; set; }

        /// <summary>
        /// 账户是否锁定
        /// </summary>

        public IsLocked? IsLocked { get; set; }

        /// <summary>
        /// 用户描述信息
        /// </summary>
        [Column(TypeName = "nvarchar(800)")]
        public string Description { get; set; }

        /// <summary>
        /// 用户的角色
        /// </summary>
        public UserRole? UserRole { get; set; }

        /// <summary>
        /// 头像 path+name+后缀
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 头像 path+name+后缀
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 头像 path+name+后缀
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string AvatorPath { get; set; }

        // 导航属性
        /// <summary>
        /// 登陆日志
        /// </summary>
        public virtual ICollection<LoginLogging> LoginLogging { get; set; }

    }
}
