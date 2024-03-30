using library_management.Data.Model;
using library_management.Data.ViewModel.Authentication;
using Microsoft.AspNetCore.Identity;

namespace library_management.Data
{
    public static class AppDbInitializer
    {
        public static async Task InitializerAsync(IServiceProvider serviceProvider,UserManager<ApplicationUser> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Author", "Member"};
            IdentityResult roleResult;
            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var email = "superadmin@gmail.com";
            var password = "Admin@123";
            if (userManager.FindByEmailAsync(email).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = "Super",
                    LastName = "admin",
                    Email = email,
                    UserName = email
                };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }
    }
}
