
using System.Linq;
using System.Threading.Tasks;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Action.Services
{
    class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger _logger;
        public UserService(UserManager<ApplicationUser> userManager,
                            RoleManager<ApplicationRole> roleManager,
                            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<UserService>();
        }
        public async Task ChangeRoleAsync(ApplicationUser user, string roleName)
        {
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<ApplicationRole> GetRole(ApplicationUser user)
        {
            var roleName = (await _userManager.GetRolesAsync(user)).First();
            return await _roleManager.FindByNameAsync(roleName);
        }
    }
    public interface IUserService
    {

        Task ChangeRoleAsync(ApplicationUser user, string roleName);

        Task<ApplicationRole> GetRole(ApplicationUser user);

    }
}