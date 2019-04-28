using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.IO;
using Auction.Models;
using Action.Services.RazorHtmlEmails;
using Auction.Models.EmailViewModels;

namespace Auction.Identity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string toUserName, string subject, string callbackUrl);
        Task SendApplyForEmailAsync(string fromPhoneNumber, string formUserName, string subject);

        Task SendInquiryAsync(InquiryViewModel inquiryViewModel);
        Task SendContactUsEmailAsync(ContactUsEmailViewModel contactUsEmailViewModel);

        Task SendMessageAsync(string fromeEmail, string fromeUsername, string toUserName, string toEmail, string messageBody, string subject);
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
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText($@"{Directory.GetCurrentDirectory()}\Identity\Templates\EmailTemplates.cshtml"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            var messageBody = builder.HtmlBody.Replace("#{UserName}", userName).Replace("#{Url}", callbackUrl);

            await SendMessageAsync("中机电", _options.AdminEmailUserName, userName, emailAdrr, messageBody, subject);
        }

        public async Task SendApplyForEmailAsync(string fromPhoneNumber, string formRealName, string subject)
        {
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText($@"{Directory.GetCurrentDirectory()}\Identity\Templates\ApplyEmailTemplates.cshtml"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            var messageBody = builder.HtmlBody.Replace("#{RealName}", formRealName)
                                        .Replace("#{PhoneNumber}", fromPhoneNumber)
                                        .Replace("#{Now}", DateTime.Now.ToShortDateString());

            await SendMessageAsync("中机电", _options.AdminEmailUserName, _options.EmailUserName, _options.EmailUserName, messageBody, subject);
        }

        public async Task SendInquiryAsync(InquiryViewModel inquiryViewModel)
        {
            var messageBody = await _razorViewToStringRenderer.RenderViewToStringAsync(
                    "/Identity/Templates/InquiryEmailTemplates.cshtml",
                        inquiryViewModel);
            await SendMessageAsync("Oog", _options.AdminEmailUserName, _options.OggEmailUserName, _options.OggEmailUserName, messageBody, "Oog运费询价");
        }

        public async Task SendOogContactUsEmailAsync(InquiryViewModel inquiryViewModel)
        {
            var messageBody = await _razorViewToStringRenderer.RenderViewToStringAsync(
                    "/Identity/Templates/InquiryEmailTemplates.cshtml",
                        inquiryViewModel);
            await SendMessageAsync("Oog", _options.AdminEmailUserName, _options.OggEmailUserName, _options.OggEmailUserName, messageBody, "Oog运费询价");
        }

        public async Task SendContactUsEmailAsync(ContactUsEmailViewModel contactUsEmailViewModel)
        {
            var messageBody = $@"姓名：{contactUsEmailViewModel.Name} &#9;  手机号：{contactUsEmailViewModel.Phone} &#9; 邮箱：{contactUsEmailViewModel.Email} <br/>
                                内容：<br/>
                                {contactUsEmailViewModel.Message}";
            await SendMessageAsync("Oog", _options.AdminEmailUserName, _options.OggEmailUserName, _options.OggEmailUserName, messageBody, "联系我们");
        }

        public async Task SendMessageAsync(string fromeUsername, string fromeEmail, string toUserName, string toEmail, string messageBody, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromeUsername, fromeEmail));
            message.To.Add(new MailboxAddress(toUserName, toEmail));
            message.Subject = subject;

            // var builder = new BodyBuilder();
            // var rootFolder = Directory.GetCurrentDirectory();
            // rootFolder = rootFolder.Substring(0,
            //             rootFolder.IndexOf(@"\Project\", StringComparison.Ordinal) + @"\Project\".Length);
            // PathToData = Path.GetFullPath(Path.Combine(rootFolder, "Data"));

            // var Parser = Parser();
            // var d = new FileStream(Path.Combine(PathToData, $"{dataFileName}.txt"), FileMode.Open);
            // var fs = new StreamReader(d, Encoding.UTF8);
            // using (StreamReader SourceReader = System.IO.File.OpenText($@"{Directory.GetCurrentDirectory()}\Identity\Templates\ApplyEmailTemplates.cshtml"))
            // {
            //     builder.HtmlBody = SourceReader.ReadToEnd();
            // }
            message.Body = new TextPart("html")
            {
                Text = messageBody
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
