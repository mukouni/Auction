using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Auction.Models.AccountViewModels;

namespace Auction.Models.ManageViewModels
{
    public class IndexViewModel
    {

        // [Required(ErrorMessage = "{0} 必须填写")]
        // [MinLength(2, ErrorMessage = "{0} 不能少于{1}个字符")]
        // [Display(Name = "真实姓名")]
        // public string RealName { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        // [Phone(ErrorMessage = "请输入有效的手机号")]
        // [Display(Name = "手机号")]
        // public string Phone { get; set; }

        // [Required(ErrorMessage = "{0} 必须填写")]
        // [StringLength(100, ErrorMessage = "{0} 不少于 {2} 个字符长度", MinimumLength = 6)]
        // [DataType(DataType.Password)]
        // [Display(Name = "密码")]
        // public string Password { get; set; }


        // [DataType(DataType.Password)]
        // [Display(Name = "确认密码")]
        // [Compare("Password", ErrorMessage = "密码不一致")]
        // public string ConfirmPassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public string AuthenticatorKey { get; set; }
    }
}
