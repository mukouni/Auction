using System.Threading.Tasks;
using Auction.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Auction.Models;
using System;

namespace Auctions.Controllers
{
    [Route("[controller]")]
    public class OogController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public OogController(
        IEmailSender emailSender,
        ILoggerFactory loggerFactory)
        {
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> InquiryEmail(InquiryViewModel inquiry)
        {
            try
            {
                await _emailSender.SendInquiryAsync(inquiry);
            }
            catch (Exception ex)
            {
                return Ok("Error");
            }
            // return Ok("Success");
            return View(inquiry);
        }
    }
}