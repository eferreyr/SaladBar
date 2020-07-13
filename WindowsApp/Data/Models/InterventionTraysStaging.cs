using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
  public partial class InterventionTraysStaging
  {
    public long BatchId { get; set; }
    public string DeviceId { get; set; }
    public long InterventionDayId { get; set; }
    public long TrayId { get; set; }
    public double? Weight { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public InterventionTraysStaging() { }

    public InterventionTraysStaging(long batchId, string deviceId, InterventionTrays it)
    {
      this.BatchId = batchId;
      this.DeviceId = deviceId;
      this.InterventionDayId = it.InterventionDayId;
      this.TrayId = it.TrayId;
      this.Weight = it.Weight;
      this.DtCreated = it.DtCreated;
      this.CreatedBy = it.CreatedBy;
      this.DtModified = it.DtModified;
      this.ModifiedBy = it.ModifiedBy;
    }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      InterventionTraysStaging its = (InterventionTraysStaging)obj;
      // ignore related data sets
      return (this.BatchId == its.BatchId) && (this.DeviceId == its.DeviceId) && (this.InterventionDayId == its.InterventionDayId) && 
             (this.TrayId == its.TrayId) && (this.Weight == its.Weight) &&
             (this.DtCreated == its.DtCreated) && (this.CreatedBy == its.CreatedBy) && (this.DtModified == its.DtModified) && (this.ModifiedBy == its.ModifiedBy);
    }

    public override int GetHashCode()
    {
      return $"{this.BatchId}{this.DeviceId}{this.InterventionDayId}{this.TrayId}{this.Weight}{this.DtCreated}".GetHashCode();
    }
  }
}
