using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} 必须填写")]
        [Phone(ErrorMessage = "请输入有效的手机号")]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} 必须填写")]
        [StringLength(100, ErrorMessage = "{0} 不少于 {2} 个字符长度", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
