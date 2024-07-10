using GameStoreNew.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStoreNew.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<GameStoreNewUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            GameStoreNewUser user = await UserManager.FindByEmailAsync("admin@mail.com");
            if (user == null)
            {
                var User = new GameStoreNewUser();
                User.Email = "admin@mail.com";
                User.UserName = "admin@mail.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin      
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GameStoreNewContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<GameStoreNewContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
            }
        }
    }
}
