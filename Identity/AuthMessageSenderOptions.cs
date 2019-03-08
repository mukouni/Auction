
namespace Auction.Identity.Options
{
    public class AuthMessageSenderOptions
    {
        public string EmailAppid { get; set; }
        public string EmailSecretKey { get; set; }

        public string AdminEmailUserName { get; set; }
        public string AdminEmailPassword { get; set; }

        public string SMSAppid { get; set; }
        public string SMSsecretKey { get; set; }
    }
}