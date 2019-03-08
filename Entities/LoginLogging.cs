using System.ComponentModel.DataAnnotations.Schema;
using Auction.Identity.Entities;

namespace Auction.Entities
{
    /// <summary>
    /// 登陆记录
    /// </summary>
    [Table("st_login_log")]
    public class LoginLogging : BaseEntity
    {

        /// <summary>
        /// 登陆 ip
        /// </summary>
        public string Ip { get; set; }

        public string Platform { get; set; }

        // 导航属性
        public virtual ApplicationUser User { get; set; }
    }
}