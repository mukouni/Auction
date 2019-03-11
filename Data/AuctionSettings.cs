namespace Auction
{
    public class AuctionSettings
    {
        public AuctionSettings(){}
        public string Secret { get; set; }
        public string AppId { get; set; }
        public string EmailAppid { get; set; }
        public string EmailKey { get; set; }

        public string EmailHost {get; set;}

        public int EmailPort {get; set;}
        public string AdminEmailUserName { get; set; }
        public string AdminEmailPassword { get; set; }
        public string SMSAppid { get; set; }
        public string SMSsecretKey { get; set; }
    }
}
