using Microsoft.AspNetCore.Identity;

namespace E_Games.Web.Infrastructure
{
    public static class RoleSeeder
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var roles = new List<string> { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }
    }
}
