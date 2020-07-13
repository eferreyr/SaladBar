using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SaladBarWeb.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaladBarWeb
{
  public static class DBInitializer
  {
    private static readonly string[] Roles = new string[] { "Admin", "Research Team Member" };

    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
      using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        var dbContext = serviceScope.ServiceProvider.GetService<IdentityDbContext>();

        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var role in Roles)
        {
          if (!await roleManager.RoleExistsAsync(role))
          {
            await roleManager.CreateAsync(new IdentityRole(role));
          }
        }
        IdentityUser user = await userManager.FindByEmailAsync("hankster@asu.edu");

        if (user != null)
        {
          IList<string> roles = await userManager.GetRolesAsync(user);

          if (!roles.Contains("Admin"))
          {
            await userManager.AddToRoleAsync(user, "Admin");
          }
        }
        else
        {
          IdentityResult result = await userManager.CreateAsync(new IdentityUser { UserName = "hankster@asu.edu", Email = "hankster@asu.edu" });
          if (result.Succeeded)
          {
            await userManager.AddToRoleAsync(user, "Admin");
          }
        }
      }
    }
  }
}
