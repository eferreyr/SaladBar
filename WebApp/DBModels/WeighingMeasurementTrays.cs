using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingMeasurementTrays
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public long InterventionDayTrayTypeId { get; set; }
        public short Quantity { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public InterventionDayTrayTypes InterventionDayTrayType { get; set; }
        public WeighingMeasurements WeighingMeasurement { get; set; }
    }
}
