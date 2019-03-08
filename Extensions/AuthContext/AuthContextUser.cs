﻿using Auction.Entities;
using System;
using System.Collections.Generic;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Extensions.AuthContext
{
    /// <summary>
    /// 登录用户上下文
    /// </summary>
    public class AuthContextUser
    {
        /// <summary>
        /// 用户GUID
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avator { get; set; }
    }
}