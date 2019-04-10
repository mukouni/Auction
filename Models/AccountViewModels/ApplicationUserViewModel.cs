using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Auction.Entities;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.AccountViewModels
{
    public class ApplicationUserViewModel
    {
        public Guid? Id { get; set; }
        // [Required(ErrorMessage = "{0} 必须填写")]
        // [EmailAddress]
        // [Display(Name = "Email")]
        // public string Email { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        [MinLength(2, ErrorMessage = "{0} 不能少于{1}个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        [MinLength(2, ErrorMessage = "{0} 不能少于{1}个字符")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        [Phone(ErrorMessage = "请输入有效的手机号")]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        public string AvatorPath { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        [StringLength(6, ErrorMessage = "请输入{2}位验证码", MinimumLength = 4)]
        [Display(Name = "验证码")]
        public string SMSCode { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        [StringLength(100, ErrorMessage = "{0} 不少于 {2} 个字符长度", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码不一致")]
        public string ConfirmPassword { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "会员到期日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeadlineAt { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "注册日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }


        [Display(Name = "登陆失败次数")]
        public int? AccessFailedCount { get; set; }


        public IsDeleted? IsDeleted { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "更新时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdatedAt { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "锁定解除时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LockoutEnd { get; set; }

        [Display(Name = "失败5次后是否可以锁定")]
        public bool LockoutEnabled { get; set; } = true;

        [Display(Name = "手机是否已验证")]
        public bool PhoneNumberConfirmed { get; set; } = false;

        [Display(Name = "邮箱是否已验证")]
        public bool? EmailConfirmed { get; set; }


        [Display(Name = "注册时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CeateedAt { get; set; }

        [Display(Name = "浏览设备记录")]
        public virtual ICollection<SearchEquipmentLog> SearchEquipmentLog { get; set; } = new List<SearchEquipmentLog>();

        public ICollection<ApplicationUserRoleViewModel> UserRoles { get; set; } = new List<ApplicationUserRoleViewModel>();


        [Display(Name = "角色")]
        public ICollection<ApplicationRoleViewModel> Roles { get; set; } = new List<ApplicationRoleViewModel>();

        [Display(Name = "角色")]
        public string Role { get; set; }

        public ICollection<SelectListItem> RoleOptions { get; set; } = new List<SelectListItem>();

    }
}
