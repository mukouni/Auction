using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction;
using Auction.Api.Extensions;
using Auction.Data;
using Auction.Entities;
using Auction.Extensions;
using Auction.Identity.Entities;
using Auction.Models.AccountViewModels;
using Auction.Models.ManageViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using X.PagedList;
using static Auction.Controllers.AccountController;

namespace Auctions.Controllers
{

    [Authorize(Roles = "Admin, Development, Staff")]
    [Route("[controller]")]
    public class SystemController : Controller
    {
        private readonly AuctionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AuctionSettings _appSettings { get; }

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public SystemController(AuctionDbContext context,
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    IMapper mapper,
                    ILoggerFactory loggerFactory,
                    IOptions<AuctionSettings> appSettings)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<SystemController>();
            _appSettings = appSettings.Value;
        }

        //
        // GET: /System/UserSearchHistory
        [HttpGet("[action]")]
        public async Task<IActionResult> UserSearchHistory(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await GetCurrentUserAsync();
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user)
            };
            return View(model);
        }

        //
        // GET: /System/UserLoginhHistory
        [HttpGet("[action]")]
        public async Task<IActionResult> UserLoginhHistory(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await GetCurrentUserAsync();
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user)
            };
            return View(model);
        }


        //
        // GET: /System/Users
        [HttpGet("[action]")]
        public async Task<IActionResult> Users(SearchApplicationUserViewModel searchApplicationUser)
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "用户列表" },
                { "href", "javascript: void(0)" }
            });
            ViewData["breadcrumb"] = breadcrumb;
            searchApplicationUser.PageSizeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "20", Text = "每页20条" },
                new SelectListItem { Value = "50", Text = "每页50条" },
                new SelectListItem { Value = "100", Text = "每页100"  },
                new SelectListItem { Value = "1000", Text = "每页1000"  },
            };

            // var user = await GetCurrentUserAsync();
            var query = _userManager.Users;
            // var query = _context.ApplicationUsers.AsQueryable<ApplicationUser>();
                    
            // var query = _context.ApplicationUsers.AsQueryable<ApplicationUser>();
            if (!string.IsNullOrEmpty(searchApplicationUser.KeyWord))
            {
                query = query.Where(x => x.RealName.Contains(searchApplicationUser.KeyWord.Trim()) || x.PhoneNumber.Contains(searchApplicationUser.KeyWord.Trim()));
            }
            query = query.OrderByDescending(e => e.LastUpdatedAt).ThenByDescending(e => e.CreatedAt).ThenBy(e => e.RealName);

            var list = query.Paged(searchApplicationUser.CurrentPage, searchApplicationUser.PageSize)
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                            .Select(u => new ApplicationUserViewModel{
                                RealName = u.RealName,
                                PhoneNumber =  u.PhoneNumber,
                                AvatorPath =  u.AvatorPath,
                                DeadlineAt = u.DeadlineAt,
                                AccessFailedCount = u.AccessFailedCount,
                                IsDeleted = u.IsDeleted,
                                CreatedAt = u.CreatedAt,
                                LastUpdatedAt = u.LastUpdatedAt,
                                UserRoles = _mapper.Map<ICollection<ApplicationUserRoleViewModel>>(u.UserRoles.ToList()),
                                Roles = _mapper.Map<ICollection<ApplicationRoleViewModel>>(u.UserRoles.Select(ur => ur.Role).ToList()),
                            });
                            // .ProjectTo<ApplicationUserViewModel>().ToList();
            // .Project().To<ApplicationUserViewModel>()

            // foreach(var user in list){
            //     user.Roles = user.UserRoles.Select(
            //          ur => new ApplicationRoleViewModel()
            //          {
            //              Id = ur.RoleId,
            //              Name = _context.Roles.Find(ur.RoleId).Name
            //          }
            //      ).ToList();
            // }
            // list.Select<ApplicationUserViewModel, List<ApplicationRoleViewModel>>(user =>
            // {
            //     return user.UserRoles.Select(
            //          ur => new ApplicationRoleViewModel()
            //          {
            //              Id = ur.RoleId,
            //              Name = _context.Roles.Find(ur.RoleId).Name
            //          }
            //      ).ToList();
            // });
            var totalCount = query.Count();

            searchApplicationUser.ApplicationUsers = new StaticPagedList<ApplicationUserViewModel>(
                    list,
                    searchApplicationUser.CurrentPage,
                    searchApplicationUser.PageSize,
                    totalCount);
            searchApplicationUser.Count = totalCount;

            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(searchApplicationUser, totalCount);

            // var myViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { { "SearchApplicationUserViewModel", searchApplicationUser } };
            // myViewData.Model = searchApplicationUser;

            // PartialViewResult result = new PartialViewResult()
            // {
            //     ViewName = "searchApplicationUserPartial",
            //     ViewData = myViewData,
            // };
            if (Request.Headers["Referer"].ToString().Contains("manage/system"))
            {
                return (IActionResult)PartialView("_IndexBodyPartial", searchApplicationUser);
            }
            return Request.IsAjaxRequest()
                ? (IActionResult)PartialView("_IndexTablePartial", searchApplicationUser)
                : View(searchApplicationUser);
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}