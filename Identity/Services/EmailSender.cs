using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Auction.Identity.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Auction.Identity.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions _options { get; }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // return Task.CompletedTask;

            return Execute(subject, message, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            SmtpClient client = new SmtpClient("smtp.263.net");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_options.AdminEmailUserName, _options.AdminEmailPassword);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_options.AdminEmailUserName);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;

            return client.SendMailAsync(mailMessage);
        }
    }
}
