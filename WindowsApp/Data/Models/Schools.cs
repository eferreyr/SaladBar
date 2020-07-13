using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Models
{
  public partial class Schools
  {
    public Schools()
    {
      InterventionDays = new HashSet<InterventionDays>();
      SchoolType = new SchoolTypes();
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string District { get; set; }
    public string Mascot { get; set; }
    public string Colors { get; set; }
    public byte[] SchoolLogo { get; set; }
    public int SchoolTypeId { get; set; }
    public string Active { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public ICollection<InterventionDays> InterventionDays { get; set; }
    public SchoolTypes SchoolType { get; set; }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      Schools s = (Schools)obj;
      bool schoolLogosEqual = false;
      if (SchoolLogo == null && s.SchoolLogo == null)
      {
        schoolLogosEqual = true;
      }
      else if ((SchoolLogo != null && s.SchoolLogo == null) || (SchoolLogo == null && s.SchoolLogo != null))
      {
        schoolLogosEqual = false;
      }
      else
      {
        schoolLogosEqual = (SchoolLogo.SequenceEqual(s.SchoolLogo));
      }
      // ignore related data sets
      return (Id == s.Id) && (Name == s.Name) && (District == s.District) && (Mascot == s.Mascot) &&
             (Colors == s.Colors) && schoolLogosEqual &&
             (SchoolTypeId == s.SchoolTypeId) && (Active == s.Active) && (DtCreated == s.DtCreated) && 
             (CreatedBy == s.CreatedBy) && (DtModified == s.DtModified) && (ModifiedBy == s.ModifiedBy);
    }
  }
}
