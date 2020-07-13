using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class Devices
  {
    public long Id { get; set; }
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string Dirty { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }
  }
}
