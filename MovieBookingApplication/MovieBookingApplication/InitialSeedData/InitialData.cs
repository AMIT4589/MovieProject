using Microsoft.AspNetCore.Identity;
using MovieBookingApplication.BookingModels;

namespace MovieBookingApplication.InitialSeedData
{
    public class InitialData
    {

        public static void Seed(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            SeedRole(scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>());
            //SeedStudents(scope.ServiceProvider.GetRequiredService<IStudentService>());
            SeedAdmin(scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>());
        }
        private static void SeedRole(RoleManager<UserRole> roleManager)
        {
            if (roleManager.Roles.Any()) return;
            roleManager.CreateAsync(new UserRole { Name = "Admin" }).GetAwaiter().GetResult();
        }

        private static void SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any()) return;
            var adminUser = new ApplicationUser
            {
                UserName = "amit",
                Email = "amit@gmail.com",
            };
            userManager.CreateAsync(adminUser, "Amit@123").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }


    }
}
