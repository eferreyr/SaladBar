using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingMeasurementTracking
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public string Info { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public WeighingMeasurements WeighingMeasurement { get; set; }
    }
}
