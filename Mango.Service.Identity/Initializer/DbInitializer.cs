using IdentityModel;
using Mango.Service.Identity.Context;
using Mango.Service.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Service.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager; 
        }
        public void Initialize()
        {

            //run pending migrations
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.MigrateAsync().GetAwaiter().GetResult();
                }
            }
            catch
            { }
            //create role if its not created
            if (!_roleManager.RoleExistsAsync(SD.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();

            }
            if (!_roleManager.RoleExistsAsync(SD.Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();

            }

            //create admin user if it not created 
            var admin_user = _userManager.FindByEmailAsync("admin@email.com").GetAwaiter().GetResult();
            if (admin_user == null)
            {
                ApplicationUser adminUser = new()
                {
                    UserName = "admin_username",
                    Email = "admin@email.com",
                    EmailConfirmed = true,
                    PhoneNumber = "18091234567",
                    FirstName = "admin_name",
                    LastName = "admin_lastname"
                };
                _userManager.CreateAsync( adminUser, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();

                var claimAdmin = _userManager.AddClaimsAsync(adminUser, new Claim[] {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Admin),
                }).Result;

            }
            //create admin user if it not created 
            var customer_user = _userManager.FindByEmailAsync("customer@email.com").GetAwaiter().GetResult();
            if (customer_user == null) 
            {
                ApplicationUser customerUser = new()
                {
                    UserName = "customer_username",
                    Email = "customer@email.com",
                    EmailConfirmed = true,
                    PhoneNumber = "18091234567",
                    FirstName = "customer_name",
                    LastName = "customer_lastname"
                };
                _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
                var claimCustomer = _userManager.AddClaimsAsync(customerUser, new Claim[] {
                new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Customer),
                }).Result;
            }
            return;
        }
    }
}
