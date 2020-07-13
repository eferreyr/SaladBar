using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
  public partial class AuditsStaging
  {
    public long BatchId { get; set; }
    public string DeviceId { get; set; }
    public string TableName { get; set; }
    public string Action { get; set; }
    public string ValuesBefore { get; set; }
    public string ValuesAfter { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public AuditsStaging() { }

    public AuditsStaging(long batchId, string deviceId, Audits a)
    {
      this.BatchId = batchId;
      this.DeviceId = deviceId;
      this.TableName = a.TableName;
      this.Action = a.Action;
      this.ValuesBefore = a.ValuesBefore;
      this.ValuesAfter = a.ValuesAfter;
      this.DtCreated = a.DtCreated;
      this.CreatedBy = a.CreatedBy;
      this.DtModified = a.DtModified;
      this.ModifiedBy = a.ModifiedBy;
    }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      AuditsStaging @as = (AuditsStaging)obj;
      // ignore related data sets
      return (this.BatchId == @as.BatchId) && (this.DeviceId == @as.DeviceId) && (this.TableName == @as.TableName) && (this.Action == @as.Action) &&
             (this.ValuesBefore == @as.ValuesBefore) && (this.ValuesAfter == @as.ValuesAfter) &&
             (this.DtCreated == @as.DtCreated) && (this.CreatedBy == @as.CreatedBy) && (this.DtModified == @as.DtModified) && (this.ModifiedBy == @as.ModifiedBy);
    }

    public override int GetHashCode()
    {
      return $"{this.BatchId}{this.DeviceId}{this.TableName}{this.Action}{this.DtCreated}".GetHashCode();
    }
  }
}
