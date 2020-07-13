using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingTraysStaging
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public string DeviceId { get; set; }
        public long WeighingId { get; set; }
        public long TrayId { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
