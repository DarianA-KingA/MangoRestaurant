using Mango.Service.Identity.Context;
using Mango.Service.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Mango.Service.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer()
        {
            
        }
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
