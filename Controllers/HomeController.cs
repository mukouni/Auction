using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auction.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text;
using Auction.Extensions.Filters;

namespace Auction.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [GenerateAntiforgeryTokenCookieForAjax]
        public IActionResult DownloadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile(string fileName = null)
        {
            fileName = "Auction.db";
            var addrUrl = $@"{Directory.GetCurrentDirectory()}\{fileName}";

            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(addrUrl, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            //文件名必须编码，否则会有特殊字符(如中文)无法在此下载。
            string encodeFilename = Encoding.UTF8.GetString(Encoding.Default.GetBytes(fileName));
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + encodeFilename);
            return new FileStreamResult(memoryStream, "application/octet-stream");
        }
    }
}
