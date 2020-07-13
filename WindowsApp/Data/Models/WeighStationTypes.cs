using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class WeighStationTypes
  {
    public WeighStationTypes()
    {
      Weighings = new HashSet<Weighings>();
    }

    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public ICollection<Weighings> Weighings { get; set; }
  }
}
