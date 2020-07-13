using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class ResearchTeamMembers
  {
    public long Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Active { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      ResearchTeamMembers s = (ResearchTeamMembers)obj;
      
      // ignore related data sets
      return (Id == s.Id) && (Email == s.Email) && (FirstName == s.FirstName) && (LastName == s.LastName) &&
             (Active == s.Active) && (DtCreated == s.DtCreated) &&
             (CreatedBy == s.CreatedBy) && (DtModified == s.DtModified) && (ModifiedBy == s.ModifiedBy);
    }
  }
}
