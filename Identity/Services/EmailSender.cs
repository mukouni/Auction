using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.IO;
using Auction.Models;
using Action.Services.RazorHtmlEmails;

namespace Auction.Identity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string toUserName, string subject, string callbackUrl);
        Task SendApplyForEmailAsync(string fromPhoneNumber, string formUserName, string subject);

        Task SendInquiryAsync(InquiryViewModel inquiryViewModel);
    }

    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        public AuctionSettings _options { get; }

        public EmailSender(IOptions<AuctionSettings> options,
            IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _options = options.Value;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public Task SendEmailAsync(string toEmail, string toUserName, string subject, string callbackUrl)
        {
            // return Task.CompletedTask;

            return ExecuteAsync(toEmail, toUserName, subject, callbackUrl);
        }

        public async Task ExecuteAsync(string emailAdrr, string userName, string subject, string callbackUrl)
        {
            // SmtpClient client = new SmtpClient(_options.EmailHost, _options.EmailPort);
            // client.Credentials = new NetworkCredential(_options.AdminEmailUserName, _options.AdminEmailPassword);
            // client.UseDefaultCredentials = false;
            // client.EnableSsl = false;

            // var builder = new StringBuilder();
            // string templatePath = $@"{Directory.GetCurrentDirectory()}\Identity\Templates";
            // IRazorLightEngine engine = EngineFactory.CreatePhysical(templatePath);
            // var model = new
            // {
            //     Username = userName,
            //     Url = callbackUrl
            // };
            // var htmlMessage = "";
            // var model = new
            // {
            //     Username = userName,
            //     Url = callbackUrl
            // };
            // MailMessage mailMessage = new MailMessage();
            // mailMessage.From = new MailAddress("it@ascendantcn.com");
            // mailMessage.To.Add(email);
            // mailMessage.Body = htmlMessage;
            // mailMessage.IsBodyHtml = true;
            // mailMessage.Subject = subject;

            // Email.DefaultSender = new SmtpSender();
            // Email.DefaultRenderer = new RazorRenderer();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("中机电", _options.AdminEmailUserName));
            message.To.Add(new MailboxAddress(userName, emailAdrr));
            message.Subject = subject;

            var builder = new BodyBuilder();
            // var rootFolder = Directory.GetCurrentDirectory();
            // rootFolder = rootFolder.Substring(0,
            //             rootFolder.IndexOf(@"\Project\", StringComparison.Ordinal) + @"\Project\".Length);
            // PathToData = Path.GetFullPath(Path.Combine(rootFolder, "Data"));

            // var Parser = Parser();
            // var d = new FileStream(Path.Combine(PathToData, $"{dataFileName}.txt"), FileMode.Open);
            // var fs = new StreamReader(d, Encoding.UTF8);
            using (StreamReader SourceReader = System.IO.File.OpenText($@"{Directory.GetCurrentDirectory()}\Identity\Templates\EmailTemplates.cshtml"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            message.Body = new TextPart("html")
            {
                Text = builder.HtmlBody.Replace("#{UserName}", userName).Replace("#{Url}", callbackUrl)
            };
            // message.HtmlBody = builder.HtmlBody;

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                // client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_options.EmailHost, _options.EmailPort, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_options.AdminEmailUserName, _options.AdminEmailPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            // return Task.CompletedTask;
        }

        public async Task SendApplyForEmailAsync(string fromPhoneNumber, string formRealName, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("中机电", _options.AdminEmailUserName));
            message.To.Add(new MailboxAddress(_options.EmailUserName, _options.EmailUserName));
            message.Subject = subject;

            var builder = new BodyBuilder();
            // var rootFolder = Directory.GetCurrentDirectory();
            // rootFolder = rootFolder.Substring(0,
            //             rootFolder.IndexOf(@"\Project\", StringComparison.Ordinal) + @"\Project\".Length);
            // PathToData = Path.GetFullPath(Path.Combine(rootFolder, "Data"));

            // var Parser = Parser();
            // var d = new FileStream(Path.Combine(PathToData, $"{dataFileName}.txt"), FileMode.Open);
            // var fs = new StreamReader(d, Encoding.UTF8);
            using (StreamReader SourceReader = System.IO.File.OpenText($@"{Directory.GetCurrentDirectory()}\Identity\Templates\ApplyEmailTemplates.cshtml"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            message.Body = new TextPart("html")
            {
                Text = builder.HtmlBody.Replace("#{RealName}", formRealName)
                                        .Replace("#{PhoneNumber}", fromPhoneNumber)
                                        .Replace("#{Now}", DateTime.Now.ToShortDateString())

            };
            // message.HtmlBody = builder.HtmlBody;

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                // client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_options.EmailHost, _options.EmailPort, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_options.AdminEmailUserName, _options.AdminEmailPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        public async Task SendInquiryAsync(InquiryViewModel inquiryViewModel)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Oog", _options.AdminEmailUserName));
            message.To.Add(new MailboxAddress(_options.OggEmailUserName, _options.OggEmailUserName));
            message.Subject = "Oog运费询价";

            var builder = new BodyBuilder();
            // var rootFolder = Directory.GetCurrentDirectory();
            // rootFolder = rootFolder.Substring(0,
            //             rootFolder.IndexOf(@"\Project\", StringComparison.Ordinal) + @"\Project\".Length);
            // PathToData = Path.GetFullPath(Path.Combine(rootFolder, "Data"));

            // var Parser = Parser();
            // var d = new FileStream(Path.Combine(PathToData, $"{dataFileName }.txt"), FileMode.Open);
            // var fs = new StreamReader(d, Encoding.UTF8);
            message.Body = new TextPart("html")
            {
                Text = await _razorViewToStringRenderer.RenderViewToStringAsync(
                    "/Identity/Templates/InquiryEmailTemplates.cshtml",
                        inquiryViewModel)
            };

            // message.HtmlBody = builder.HtmlBody;

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                // client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_options.EmailHost, _options.EmailPort, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_options.AdminEmailUserName, _options.AdminEmailPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
