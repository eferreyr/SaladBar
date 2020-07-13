using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class ImageTypes
    {
        public ImageTypes()
        {
            WeighingMeasurements = new HashSet<WeighingMeasurements>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<WeighingMeasurements> WeighingMeasurements { get; set; }
    }
}
