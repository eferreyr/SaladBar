using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Models
{
  public partial class WeighingsStaging
  {
    public long BatchId { get; set; }
    public string DeviceId { get; set; }
    public long WeighingId { get; set; }
    public long InterventionDayId { get; set; }
    public int WeighStationTypeId { get; set; }
    public byte[] Picture { get; set; }
    public string SaladDressing { get; set; }
    public string Milk { get; set; }
    public string UniqueSituation { get; set; }
    public string Empty { get; set; }
    public string Multiple { get; set; }
    public string Seconds { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public WeighingsStaging() { }

    public WeighingsStaging(long batchId, string deviceId, Weighings w)
    {
      this.BatchId = batchId;
      this.DeviceId = deviceId;
      this.WeighingId = w.Id;
      this.InterventionDayId = w.InterventionDayId;
      this.WeighStationTypeId = w.WeighStationTypeId;
      this.Picture = w.Picture;
      this.SaladDressing = w.SaladDressing;
      this.Milk = w.Milk;
      this.UniqueSituation = w.UniqueSituation;
      this.Empty = w.Empty;
      this.Multiple = w.Multiple;
      this.Seconds = w.Seconds;
      this.DtCreated = w.DtCreated;
      this.CreatedBy = w.CreatedBy;
      this.DtModified = w.DtModified;
      this.ModifiedBy = w.ModifiedBy;
    }

    public override bool Equals(object obj)
    {
      // Check for null values and compare run-time types.
      if (obj == null || GetType() != obj.GetType())
        return false;

      WeighingsStaging ws = (WeighingsStaging)obj;
      bool pictureEqual = false;
      if (this.Picture == null && ws.Picture == null)
      {
        pictureEqual = true;
      }
      else if ((this.Picture != null && ws.Picture == null) || (this.Picture == null && ws.Picture != null))
      {
        pictureEqual = false;
      }
      else
      {
        pictureEqual = (this.Picture.SequenceEqual(ws.Picture));
      }
      // ignore related data sets
      return (this.BatchId == ws.BatchId) && (this.DeviceId == ws.DeviceId) && (this.WeighingId == ws.WeighingId) &&
             (this.InterventionDayId == ws.InterventionDayId) && (this.WeighStationTypeId == ws.WeighStationTypeId) && pictureEqual &&
             (this.SaladDressing == ws.SaladDressing) && (this.Milk == ws.Milk) && (this.UniqueSituation == ws.UniqueSituation) &&
             (this.Empty == ws.Empty) && (this.Multiple == ws.Multiple) && (this.Seconds == ws.Seconds) &&
             (this.DtCreated == ws.DtCreated) && (this.CreatedBy == ws.CreatedBy) && (this.DtModified == ws.DtModified) && (this.ModifiedBy == ws.ModifiedBy);
    }

    public override int GetHashCode()
    {
      return $"{this.BatchId}{this.DeviceId}{this.WeighingId}{this.InterventionDayId}{this.DtCreated}".GetHashCode();
    }
  }
}
