using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Entities.Enums;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Auction.Data;

namespace Auction.Identity.Extensions
{
    public static class UserManagerExtensions
    {

        public static Task<string> GeneratePhoneNumberTokenAsync(this UserManager<ApplicationUser> userManager, int? strLength = 4)
        {
            return Task.Run(() =>
             {
                 return GenerateToken(strLength);
             });
        }
        
        public static ApplicationUser FindByPhoneNumber(this UserManager<ApplicationUser> userManager, AuctionDbContext context, string phoneNumber)
        {
                return context.Users.SingleOrDefault(u => u.PhoneNumber == phoneNumber);
        }

        private static string GenerateToken(int? VcodeNum)
        {
            //验证码可以显示的字符集合  
            string Vchar = "1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(new Char[] { ',' }); //拆分成数组   
            string code = ""; //产生的随机数  
            int temp = -1; //记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks)); //初始化随机类  
                }
                int t = rand.Next(8); //获取随机数  
                if (temp != -1 && temp == t)
                {
                    return GenerateToken(VcodeNum); //如果获取的随机数重复，则递归调用 
                }
                temp = t; //把本次产生的随机数记录起来  
                code += VcArray[t]; //随机数的位数加一  
            }
            return code;
        }

        public static void AddUserRole(this UserManager<ApplicationUser> userManager, string phoneNumber, string role)
        {
           Array allRoles = Enum.GetValues(typeof(CommonEnum.UserRole));


            // foreach (int role in allRoles)
            // {
            //     if (!await roleManager.RoleExistsAsync(role.ToString()))
            //     {
            //         await roleManager.CreateAsync(new IdentityRole
            //         {
            //             Name = role.ToString(),
            //             NormalizedName = role.ToString().ToUpper()
            //         });
            //     }
            // }
        }
    }
}