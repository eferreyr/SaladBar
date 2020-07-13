using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaladBarWeb.Models.UserManagementViewModels
{
  public class EditUserViewModel
  {
    public string Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    public string Email { get; set; }

    public bool Active { get; set; }

    [Display(Name = "User Roles")]
    public List<SelectListItem> UserRoles { get; set; }
    

    public EditUserViewModel()
    {
      this.UserRoles = new List<SelectListItem>();
    }
  }
}
