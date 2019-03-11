using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Auction.Identity.Entities;
using Auction.Entities;
using Microsoft.AspNetCore.Identity;
using Auction.Identity.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Auction.Models.AccountViewModels;
using Auction.Api.Extensions;
using Auction.Identity;

namespace Auction.Controllers.Api
{
    [Area("api")]
    [Route("[area]/[controller]")]
    public class AccountController : ApiController
    {
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(AppIdentityDbContext context,
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             IMapper mapper,
             ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // POST: /Account/Login
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var response = ResponseModelFactory.CreateInstance;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                string username = _context.Users.Where(u => u.PhoneNumber == model.Phone).Select(u => u.UserName).ToList().First();
                var result = await _signInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return Ok(response);
                }
                if (result.RequiresTwoFactor)
                {
                    response.SetFailed("需要二次验证登陆");
                    response.SetData(new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    return Ok(response);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");

                    response.SetFailed("账号已被锁定");
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("Invalid login attempt.");
                    return Ok(response);
                }
            }

            response.SetFailed("需要二次验证登陆");
            return Ok(response);
        }
    }
}