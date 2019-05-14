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
using Auction.Data;
using Auction.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Auction.Api.Auth;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Auction.Controllers.Api
{
    [Area("api")]
    [Route("[area]/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly AuctionSettings _appSettings;
        private readonly AuctionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(IOptions<AuctionSettings> appSettings,
        AuctionDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMapper mapper,
            ILoggerFactory loggerFactory)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // POST: /Account/Login
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var response = ResponseModelFactory.CreateInstance;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                string username = _context.Users.Where(u => u.PhoneNumber == model.PhoneNumber).Select(u => u.UserName).FirstOrDefault();
                if (username == null)
                {
                    response.SetFailed("没有找到手机号");
                    return Ok(response);
                }
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    response.SetFailed("没有找到用户");
                    return Ok(response);
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                // var result = await _signInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    // var claimsIdentity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim("avatar", ""),
                        new Claim("displayName", user.RealName),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
                    };
                    response.SetData(JwtBearerAuthenticationExtension.GenerateJwtToken(_appSettings, claims));
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

        [Authorize]
        [HttpGet]
        public object Protected()
        {
            return "Protected area";
        }

    }
}