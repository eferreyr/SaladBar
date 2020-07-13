using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class InterventionDays
  {
    public InterventionDays()
    {
      InterventionTrays = new HashSet<InterventionTrays>();
      Weighings = new HashSet<Weighings>();
    }

    public long Id { get; set; }
    public long SchoolId { get; set; }
    public DateTime DtIntervention { get; set; }
    public string InterventionFinished { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public Schools School { get; set; }
    public ICollection<InterventionTrays> InterventionTrays { get; set; }
    public ICollection<Weighings> Weighings { get; set; }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      InterventionDays i = (InterventionDays)obj;
      // ignore related data sets
      return (Id == i.Id) && (SchoolId == i.SchoolId) && (DtIntervention == i.DtIntervention) &&
             (InterventionFinished == i.InterventionFinished) && (DtCreated == i.DtCreated) &&
             (CreatedBy == i.CreatedBy) && (DtModified == i.DtModified) && (ModifiedBy == i.ModifiedBy);
    }
  }
}
