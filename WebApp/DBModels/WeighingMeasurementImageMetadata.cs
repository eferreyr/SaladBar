using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingMeasurementImageMetadata
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public int ImageMetadataId { get; set; }
        public string Selected { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
        public short Value { get; set; }

        public ImageMetadata ImageMetadata { get; set; }
        public WeighingMeasurements WeighingMeasurement { get; set; }
    }
}
