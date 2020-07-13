using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SaladBarWeb.Models
{
  // Add profile data for application users by adding properties to the ApplicationUser class
  public class ApplicationUser : IdentityUser
  {
    // public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
  }
}
