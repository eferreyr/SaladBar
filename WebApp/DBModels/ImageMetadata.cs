using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class ImageMetadata
    {
        public ImageMetadata()
        {
            WeighingMeasurementImageMetadata = new HashSet<WeighingMeasurementImageMetadata>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<WeighingMeasurementImageMetadata> WeighingMeasurementImageMetadata { get; set; }
    }
}
