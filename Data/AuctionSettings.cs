using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Auction
{
    public class AuctionSettings
    {
        public AuctionSettings() { }

        //谁颁发的
        public string Issuer { get; set; }

        //颁发给谁
        public string Audience { get; set; }

        //令牌密码
        public string SecretKey { get; set; }

        //对称秘钥
        public SymmetricSecurityKey IssuerSigningKey { 
            get {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));
            }
            set{}
        } 

        //数字签名 对称公钥
        public SigningCredentials SigningCredentials
        {
            get
            {
                return new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            }
        }

        public string JwtAppId { get; set; }
        public string EmailAppid { get; set; }
        public string EmailKey { get; set; }

        public string EmailHost { get; set; }

        public int EmailPort { get; set; }
        public string AdminEmailUserName { get; set; }
        public string AdminEmailPassword { get; set; }
        public string SMSAppid { get; set; }
        public string SMSsecretKey { get; set; }
    }
}
