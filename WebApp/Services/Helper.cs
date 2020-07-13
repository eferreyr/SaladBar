using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaladBarWeb.Services
{
  public class Helper
  {
    public Helper() {}

    public string GetUserFullName(string email, bool shortLast = true)
    {
      using (var dbContext = new AppDbContext())
      {
        var fullName = email;
        var researchTeamMember = dbContext.ResearchTeamMembers
          .Where(x => x.Email == email)
          .FirstOrDefault();

        if (researchTeamMember != null)
        {
          var lastName = shortLast ? $"{researchTeamMember.LastName[0]}." : researchTeamMember.LastName;
          fullName = $"{researchTeamMember.FirstName} {lastName}";
        }

        return fullName;
      }
    }

    public string GetUserFirstName(string email)
    {
      using (var dbContext = new AppDbContext())
      {
        var firstName = email;
        var researchTeamMember = dbContext.ResearchTeamMembers
          .Where(x => x.Email == email)
          .FirstOrDefault();

        if (researchTeamMember != null)
        {
          firstName = researchTeamMember.FirstName;
        }

        return firstName;
      }
    }
  }
}
