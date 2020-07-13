using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SaladBarWeb.Models
{
  public class ApplicationRole : IdentityRole
  {
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName)
    {
      Name = roleName;
    }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
  }
}
