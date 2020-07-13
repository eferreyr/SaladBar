using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class SchoolTypes
  {
    public SchoolTypes()
    {
      Schools = new HashSet<Schools>();
    }

    public long Id { get; set; }
    public string Type { get; set; }
    public string Grades { get; set; }
    public string RandomSampleSize { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public ICollection<Schools> Schools { get; set; }
  }
}
