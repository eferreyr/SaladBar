using System;
using System.Collections.Generic;

namespace Data.Models
{
  public partial class Audits
  {
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public string TableName { get; set; }
    public string Action { get; set; }
    public string ValuesBefore { get; set; }
    public string ValuesAfter { get; set; }
    public string Dirty { get; set; }
    public DateTime DtCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? DtModified { get; set; }
    public string ModifiedBy { get; set; }

    public override string ToString()
    {
      return $"Id: {Id}\n" +
        $"DeviceId: {DeviceId}\n" +
        $"TableName: {TableName}\n" +
        $"Action: {Action}\n" +
        $"ValuesBefore: {ValuesBefore}\n" +
        $"ValuesAfter: {ValuesAfter}\n" +
        $"Dirty: {Dirty}\n" +
        $"DtCreated: {DtCreated}\n" +
        $"CreatedBy: {CreatedBy}\n" +
        $"DtModified: {DtModified}\n" +
        $"ModifiedBy: {ModifiedBy}\n";
    }

    public string ToStringForWeighing()
    {
      return $"Id: {Id}\n" +
        $"DeviceId: {DeviceId}\n" +
        $"TableName: {TableName}\n" +
        $"Action: {Action}\n" +
        $"ValuesBefore: Removed in the logging to save space, the data is still available in the Audits table\n" +
        $"ValuesAfter: Removed in the logging to save space, the data is still available in the Audits table\n" +
        $"Dirty: {Dirty}\n" +
        $"DtCreated: {DtCreated}\n" +
        $"CreatedBy: {CreatedBy}\n" +
        $"DtModified: {DtModified}\n" +
        $"ModifiedBy: {ModifiedBy}\n";
    }
  }
}
