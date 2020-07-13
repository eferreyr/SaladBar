using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SaladBarWeb.Models.UserManagementViewModels
{
  public class UserManagementAddRoleViewModel
  {
    public string UserId { get; set; }
    public string Email { get; set; }
    public string NewRole { get; set; }
    public List<IdentityRole> Roles { get; set; }
  }
}
