using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class InterventionDayTrayTypes
    {
        public InterventionDayTrayTypes()
        {
            WeighingMeasurementTrays = new HashSet<WeighingMeasurementTrays>();
        }

        public long Id { get; set; }
        public int TrayTypeId { get; set; }
        public long InterventionDayId { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public InterventionDays InterventionDay { get; set; }
        public TrayTypes TrayType { get; set; }
        public ICollection<WeighingMeasurementTrays> WeighingMeasurementTrays { get; set; }
    }
}
