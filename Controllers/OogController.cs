using System.Threading.Tasks;
using Auction.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Auction.Models;
using System;
using System.IO;
using CsvHelper;
using Auction.Models.EmailViewModels;
using Microsoft.AspNetCore.Cors;

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
        [EnableCors("*")]
        public async Task<IActionResult> InquiryEmail(InquiryViewModel inquiry)
        {
            try
            {
                await _emailSender.SendInquiryAsync(inquiry);
                //     using (var writer = new StreamWriter("path\\to\\file.csv"))
                //     using (var csv = new CsvWriter(writer))
                //     {
                //         csv.WriteRecords(inquiry);
                //     }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Ok(ex.Message);
                // return Ok("Error");
            }
            return Ok("success");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        [EnableCors("*")]
        public async Task<IActionResult> ContactUsEmail(ContactUsEmailViewModel contactUsEmailViewModel)
        {
            await _emailSender.SendContactUsEmailAsync(contactUsEmailViewModel);
            return Ok("success");
        }
    }
}