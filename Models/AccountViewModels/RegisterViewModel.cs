using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auction.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        // [Required(ErrorMessage = "{0} 必须填写")]
        // [EmailAddress]
        // [Display(Name = "Email")]
        // public string Email { get; set; }

        [Required(ErrorMessage = "{0} 必须填写")]
        [MinLength(2, ErrorMessage = "{0} 不能少于{1}个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} 必须填写")]
        [MinLength(2, ErrorMessage = "{0} 不能少于{1}个字符")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        [Required(ErrorMessage = "{0} 必须填写")]
        [Phone(ErrorMessage = "请输入有效的手机号")]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} 必须填写")]
        [StringLength(6, ErrorMessage = "请输入{2}位验证码", MinimumLength = 4)]
        [Display(Name = "验证码")]
        public string SMSCode { get; set; }

        [Required(ErrorMessage = "{0} 必须填写")]
        [StringLength(100, ErrorMessage = "{0} 不少于 {2} 个字符长度", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码不一致")]
        public string ConfirmPassword { get; set; }

        
        [DataType(DataType.Date)]
        [Display(Name = "合同到期日期")]
        public DateTime? DeadlineAt { get; set; }

         /// <summary>
        /// 发送类型： Register ResetPassword
        /// </summary>
        public string SMSType { get; set; }
    }
}
