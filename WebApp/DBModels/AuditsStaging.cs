using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class AuditsStaging
    {
        public long Id { get; set; }
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
    }
}
