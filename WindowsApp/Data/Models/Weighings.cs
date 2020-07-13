using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class Weighings
  {
    public Weighings()
    {
      WeighingTrays = new HashSet<WeighingTrays>();
    }

    public long Id { get; set; }
    public long InterventionDayId { get; set; }
    public int WeighStationTypeId { get; set; }
    public byte[] Picture { get; set; }
    public string SaladDressing { get; set; }
    public string Milk { get; set; }
    public string UniqueSituation { get; set; }
    public string Empty { get; set; }
    public string Multiple { get; set; }
    public string Seconds { get; set; }
    public string Dirty { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public InterventionDays InterventionDay { get; set; }
    public WeighStationTypes WeighStationType { get; set; }
    public ICollection<WeighingTrays> WeighingTrays { get; set; }
  }
}
