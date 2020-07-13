using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SaladBarWeb.Data;
using SaladBarWeb.DBModels;
using SaladBarWeb.Models;
using SaladBarWeb.Models.UserManagementViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Controllers
{
  [Authorize(Roles = "Admin")]
  public class UserManagementController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger _logger;
    private IdentityUser _currentUser;
    private IdentityUser CurrentUser
    {
      get
      {
        if (_currentUser == null)
        {
          _currentUser = _userManager.GetUserAsync(User).Result;
        }

        return _currentUser;
      }
    }

    public UserManagementController(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserManagementController> logger)
    {
      _context = context;
      _userManager = userManager;
      _roleManager = roleManager;
      _logger = logger;
    }

    public IActionResult Index()
    {
      //var users = _context.AspNetUsers
      //  .Join(_context.ResearchTeamMembers, anu => anu.Email, rtm => rtm.Email, (anu, rtm) => new { anu, rtm })
      //  .DefaultIfEmpty()
      //  .Select(x => new User
      //  {
      //    Email = x.anu.Email,
      //    Roles = string.Join(", ", _context.AspNetUserRoles
      //      .Where(y => y.UserId == x.anu.Id)
      //      .Join(_context.AspNetRoles, anur => anur.RoleId, anr => anr.Id, (anur, anr) => anr.Name)
      //      .Select(z => z)),
      //    FirstName = x.rtm.FirstName,
      //    LastName = x.rtm.LastName 
      //  });

      var users = _context.AspNetUsers
        .GroupJoin(
            _context.ResearchTeamMembers,
            anu => anu.Email,
            rtm => rtm.Email,
            (anu, rtm) => new { anu, rtm })
        .SelectMany(
            x => x.rtm.DefaultIfEmpty(),
            (x, y) => new User
            {
              FirstName = y.FirstName,
              LastName = y.LastName,
              Email = x.anu.Email,
              Active = y.Active == "Y",
              Roles = string.Join(", ", _context.AspNetUserRoles
                .Where(z => z.UserId == x.anu.Id)
                .Join(_context.AspNetRoles, anur => anur.RoleId, anr => anr.Id, (anur, anr) => anr.Name)
                .Select(z => z))
            })
        .OrderBy(x => x.FirstName)
        .ThenBy(x => x.LastName);

      var vm = new UserManagementIndexViewModel
      {
        Users = users.ToList()
      };

      return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult AddUser()
    {
      UserViewModel model = new UserViewModel();
      return PartialView("_AddUser", model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddUser(UserViewModel model)
    {
      if (ModelState.IsValid)
      {
        IdentityUser user = new IdentityUser
        {
          UserName = model.Email,
          Email = model.Email
        };
        // TODO: Match this part of the code with AccountController line 314.
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          if (await AddToResearchTeamMember(model))
          {
            _logger.LogInformation("User created a new account with password.");

            var roleResult = await _userManager.AddToRoleAsync(user, "Research Team Member");
            if (roleResult.Succeeded)
            {
              return RedirectToAction("Index");
            }
          }
        }
      }
      return View("_AddUser", model);
    }

    private async Task<bool> AddToResearchTeamMember(UserViewModel model)
    {
      // Verify the email does not exist before inserting into the database
      var emailExist = _context.ResearchTeamMembers
        .Where(x => x.Email == model.Email)
        .Any();

      if (emailExist)
      {
        ModelState.AddModelError(string.Empty, "Email already exist in Research Team Member.");
        _logger.LogError("AspNetUser was created, but Research Team Member fail to create because it's a duplicated email");
        return false;
      }
      else
      {
        var researchTeamMember = new ResearchTeamMembers
        {
          Email = model.Email,
          FirstName = model.FirstName,
          LastName = model.LastName,
          CreatedBy = CurrentUser.Email,
          Active = "Y"
        };

        _context.Add(researchTeamMember);
        await _context.SaveChangesAsync();

        return true;
      }
    }

    [HttpGet]
    public async Task<IActionResult> EditUser(string email)
    {
      EditUserViewModel model = new EditUserViewModel();

      if(!string.IsNullOrEmpty(email))
      {
        IdentityUser user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
          model.Email = user.Email;
          var userRoles = await _userManager.GetRolesAsync(user);
          var roles = _roleManager.Roles.ToList();

          foreach (var role in roles)
          {
            model.UserRoles.Add(new SelectListItem { Text = role.Name, Value = role.Name, Selected = userRoles.Any(x => x == role.Name) });
          }

          var researchTeamMember = GetResearchTeamMember(email);
          if (researchTeamMember != null)
          {
            model.FirstName = researchTeamMember.FirstName;
            model.LastName = researchTeamMember.LastName;
            model.Active = researchTeamMember.Active == "Y";
          }
        }
      }
      return PartialView("_EditUser", model);
    }

    private ResearchTeamMembers GetResearchTeamMember(string email)
    {
      var researchTeamMember = _context.ResearchTeamMembers
        .Where(x => x.Email == email)
        .FirstOrDefault();

      return researchTeamMember;
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(string oldEmail, EditUserViewModel model)
    {
      if (ModelState.IsValid)
      {
        IdentityUser user = await _userManager.FindByEmailAsync(oldEmail);
        if (user != null)
        {
          user.Email = model.Email;
          user.UserName = model.Email;

          IdentityResult result = await _userManager.UpdateAsync(user);
          
          if (result.Succeeded)
          {
            // JY: The follwing 10ish lines are inefficient, but it's the easiest way
            var userRoles = await _userManager.GetRolesAsync(user);
            result = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (result.Succeeded)
            {
              var researchTeamMember = GetResearchTeamMember(oldEmail);
              if (researchTeamMember != null)
              {
                researchTeamMember.FirstName = model.FirstName;
                researchTeamMember.LastName = model.LastName;
                researchTeamMember.Active = model.Active ? "Y" : "N";
                researchTeamMember.DtModified = DateTime.Now;
                researchTeamMember.ModifiedBy = CurrentUser.Email;

                _context.Attach(researchTeamMember);
                await _context.SaveChangesAsync();

                if (model.Active)
                {
                  var selectedRoles = model.UserRoles
                    .Where(x => x.Selected)
                    .Select(x => x.Value)
                    .ToList();

                  result = await _userManager.AddToRolesAsync(user, selectedRoles);
                  if (result.Succeeded)
                  {
                    return RedirectToAction("Index");
                  }
                }
                else
                {
                  return RedirectToAction("Index");
                }
              }
            }
          }
        }
      }
      return PartialView("_EditUser", model);
    }

    /***************************************************
     * old
     * *************************************************/
    [HttpGet]
    public async Task<IActionResult> AddRole(string email)
    {
      var user = await GetUserByEmailAsync(email);
      var vm = new UserManagementAddRoleViewModel
      {
        Roles = GetAllRoles(),
        Email = email,
        UserId = user.Id
      };

      return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(UserManagementAddRoleViewModel rvm)
    {
      if (ModelState.IsValid)
      {
        var user = await GetUserByEmailAsync(rvm.Email);
        var result = await _userManager.AddToRoleAsync(user, rvm.NewRole);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(error.Code, error.Description);
        }
      }
      rvm.Roles = GetAllRoles();
      return View(rvm);
    }

    private async Task<IdentityUser> GetUserByEmailAsync(string email) => 
      await _userManager.FindByEmailAsync(email);

    private  List<IdentityRole> GetAllRoles() => 
      _roleManager.Roles.OrderBy(r => r.Name).ToList();
  }
}