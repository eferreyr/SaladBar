using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class Devices
    {
        public long Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
        public long Seed { get; set; }
    }
}
