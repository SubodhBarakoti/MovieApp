using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Enums;
using Entities;

namespace DataAccessLayer.Persistance.Seed
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext,
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (!_roleManager.RoleExistsAsync(UserRole.Super_Admin.ToString()).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(UserRole.Super_Admin.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRole.Admin.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRole.Basic_User.ToString())).GetAwaiter().GetResult();
            }

            var user = new Users
            {
                //Id=Guid.NewGuid().ToString(),
                FirstName="Subodh",
                LastName="Barakoti",
                UserName = "subodhbarakoti17@gmail.com",
                NormalizedUserName = "SUBODHBARAKOTI17@GMAIL.COM",
                Email = "subodhbarakoti17@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "SUBODHBARAKOTI17@GMAIL.COM",
                LockoutEnabled = true,
                PhoneNumber = "9840530961",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var userManager = _userManager.CreateAsync(user, "Password@17").GetAwaiter().GetResult();

            var result = _dbContext.Users.FirstOrDefault(u => u.Email == "subodhbarakoti17@gmail.com") ?? throw new NullReferenceException();

            _userManager.AddToRoleAsync(result, UserRole.Super_Admin.ToString()).GetAwaiter().GetResult();

            await _dbContext.SaveChangesAsync();
        }
    }
}
