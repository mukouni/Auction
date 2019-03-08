using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Auction.Identity.Options;

namespace Auction.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }

    public class SmsSender : ISmsSender
    {
        public SmsSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions _options { get; }  // set only via Secret Manager


        public Task SendSmsAsync(string number, string message)
        {
            return Execute(number, message);
        }

        public Task Execute(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            var accountSid = _options.SMSAppid;
            // Your Auth Token from twilio.com/console
            var authToken = _options.SMSsecretKey;

            // TwilioClient.Init(accountSid, authToken);

            // return MessageResource.CreateAsync(
            //   to: new PhoneNumber(number),
            //   from: new PhoneNumber(Options.SMSAccountFrom),
            //   body: message);
            return Task.FromResult(0);
        }
    }
}