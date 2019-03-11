using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Auction.Identity.Services;
using System;

namespace Auction.Identity.Extensions
{
    public static class EmailSenderExtensions
    {
        // public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, Func<EmailSender> sender, string email, string link)
        // {
        //     return sender().SendEmailAsync(email, "Confirm your email",
        //         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.");
        // }

        // public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        // {
        //     return emailSender.SendEmailAsync(email, "Reset Password",
        //         $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        // }

        
    }
}
