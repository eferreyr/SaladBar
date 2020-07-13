using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingMeasurementMenuItems
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public long MenuItemId { get; set; }
        public string Selected { get; set; }
        public short? Quantity { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public MenuItems MenuItem { get; set; }
        public WeighingMeasurements WeighingMeasurement { get; set; }
    }
}
