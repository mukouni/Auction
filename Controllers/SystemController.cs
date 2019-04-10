using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction;
using Auction.Api.Extensions;
using Auction.Data;
using Auction.Entities;
using Auction.Entities.Enums;
using Auction.Extensions;
using Auction.Extensions.Alerts;
using Auction.Identity.Entities;
using Auction.Identity.Services;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AuctionSettings _appSettings { get; }

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public SystemController(AuctionDbContext context,
                    UserManager<ApplicationUser> userManager,
                    RoleManager<ApplicationRole> roleManager,
                    SignInManager<ApplicationUser> signInManager,
                    IEmailSender emailSender,
                    ISmsSender smsSender,
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
        // GET: /System/Index
        [HttpGet("[action]")]
        public IActionResult Index(ManageMessageId? message = null)
        {
            return View();
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


        // GET: /System/Users
        [HttpGet("[action]")]
        public IActionResult Users(SearchApplicationUserViewModel searchApplicationUser)
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
                            .Select(u => new ApplicationUserViewModel
                            {
                                Id = u.Id,
                                RealName = u.RealName,
                                PhoneNumber = u.PhoneNumber,
                                AvatorPath = u.AvatorPath,
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
                return (IActionResult)PartialView("_UsersBodyPartial", searchApplicationUser);
            }
            return Request.IsAjaxRequest()
                ? (IActionResult)PartialView("_UsersTablePartial", searchApplicationUser)
                : View(searchApplicationUser);
        }

        // GET: /System/UserNew
        [HttpGet("[action]")]
        public IActionResult UserNew(string returnUrl = null)
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "用户新建" },
                { "href", "javascript: void(0)" }
            });
            ViewData["breadcrumb"] = breadcrumb;

            ApplicationUserViewModel model = new ApplicationUserViewModel();
            model.RoleOptions = _context.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // POST: System/UserNew
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserNew(ApplicationUserViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { PhoneNumber = model.PhoneNumber, UserName = model.UserName };

                if (_context.Users.Where(u => u.PhoneNumber == model.PhoneNumber).Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "手机号已被占用!");
                    return View(model);
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                bool guestRoleExist = await _roleManager.RoleExistsAsync(ApplicationRole.Guest);
                if (!guestRoleExist)
                {
                    _logger.LogInformation("Adding " + ApplicationRole.Guest + " role");
                    await _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = ApplicationRole.Guest,
                        NormalizedName = ApplicationRole.Guest.ToUpper()
                    });
                }

                await _userManager.AddToRoleAsync(
                    await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber),
                    model.Role);
                if (result.Succeeded)
                {
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(Users));
                }
                AddErrors(result);
            }
            return View(model);
        }

        // GET: /System/UserEdit
        [HttpGet("[action]")]
        public async Task<IActionResult> UserEdit(Guid? id, string returnUrl = null)
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "用户修改" },
                { "href", "javascript: void(0)" }
            });
            ViewData["breadcrumb"] = breadcrumb;
            ViewData["ReturnUrl"] = returnUrl;

            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.ApplicationUsers.FindAsync(id);
            var userVM = _mapper.Map<ApplicationUserViewModel>(user);
            userVM.RoleOptions = _context.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
            userVM.Role = (await _userManager.GetRolesAsync(user)).ToArray().First();

            return View(userVM);
        }

        // POST: System/UserNew
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(ApplicationUserViewModel model, string returnUrl = null)
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "用户修改" },
                { "href", "javascript: void(0)" }
            });
            ViewData["breadcrumb"] = breadcrumb;
            ViewData["ReturnUrl"] = returnUrl;

            model.RoleOptions = _context.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            if (ModelState.IsValid)
            {
                var user = await _context.ApplicationUsers.FindAsync(model.Id);
                // var user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (!string.IsNullOrEmpty(model.Password))
                {
                    // user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    await _userManager.RemovePasswordAsync(user);
                    var changeResult = await _userManager.AddPasswordAsync(user, model.Password);
                    if (!changeResult.Succeeded)
                    {
                        AddErrors(changeResult);
                        return View(model);
                    }
                }
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await changeRoleAsync(user, model.Role);
                    await _context.SaveChangesAsync();
                }

                // var ecurityStamp = await _userManager.UpdateSecurityStampAsync(user).Result;
                var FormUser = _mapper.Map<ApplicationUser>(model);
                
                user.RealName = FormUser.RealName;
                user.DeadlineAt = FormUser.DeadlineAt;
                user.Email = FormUser.Email;
                user.IsDeleted = FormUser.IsDeleted == CommonEnum.IsDeleted.Yes ? CommonEnum.IsDeleted.Yes : CommonEnum.IsDeleted.No;
                user.LockoutEnabled = FormUser.LockoutEnabled;
                user.LockoutEnd = FormUser.LockoutEnd;
                user.AvatorPath = FormUser.AvatorPath;
                
                var result = await _userManager.UpdateAsync(user);
                _context.SaveChanges();
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return RedirectToLocal("/system/users").WithSuccess("", "修改成功");
            }
            return View(model);
        }

        // 删除用户
        [HttpPost("[action]")]
        public async Task<IActionResult> UserDelete(Guid? id)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            user.IsDeleted = CommonEnum.IsDeleted.Yes;
            _context.ApplicationUsers.Attach(user);
            _context.Entry(user).Property(x => x.IsDeleted).IsModified = true;
            await _context.SaveChangesAsync();
            response.SetData(new { id = id });
            return Ok(response);
        }

        // 成为会员
        [HttpPost("[action]")]
        public async Task<IActionResult> BecomeMember(Guid? id)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            await changeRoleAsync(user, ApplicationRole.Member);
            response.SetData(new { id = id });
            return Ok(response);
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
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(SystemController.Index), "Home");
            }
        }

        private async Task changeRoleAsync(ApplicationUser user, string roleName)
        {
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, roleName);
        }
        #endregion
    }
}