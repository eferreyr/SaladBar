using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
  public partial class WeighingTraysStaging
  {
    public long BatchId { get; set; }
    public string DeviceId { get; set; }
    public long WeighingId { get; set; }
    public long TrayId { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public WeighingTraysStaging() { }

    public WeighingTraysStaging(long batchId, string deviceId, WeighingTrays wt)
    {
      this.BatchId = batchId;
      this.DeviceId = deviceId;
      this.WeighingId = wt.WeighingId;
      this.TrayId = wt.TrayId;
      this.DtCreated = wt.DtCreated;
      this.CreatedBy = wt.CreatedBy;
      this.DtModified = wt.DtModified;
      this.ModifiedBy = wt.ModifiedBy;
    }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      WeighingTraysStaging wts = (WeighingTraysStaging)obj;
      // ignore related data sets
      return (this.BatchId == wts.BatchId) && (this.DeviceId == wts.DeviceId) && (this.WeighingId == wts.WeighingId) && (this.TrayId == wts.TrayId) &&
             (this.DtCreated == wts.DtCreated) && (this.CreatedBy == wts.CreatedBy) && (this.DtModified == wts.DtModified) && (this.ModifiedBy == wts.ModifiedBy);
    }

    public override int GetHashCode()
    {
      return $"{this.BatchId}{this.DeviceId}{this.WeighingId}{this.TrayId}{this.DtCreated}".GetHashCode();
    }
  }
}
