using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingMeasurements
    {
        public WeighingMeasurements()
        {
            WeighingMeasurementImageMetadata = new HashSet<WeighingMeasurementImageMetadata>();
            WeighingMeasurementMenuItems = new HashSet<WeighingMeasurementMenuItems>();
            WeighingMeasurementTracking = new HashSet<WeighingMeasurementTracking>();
            WeighingMeasurementTrays = new HashSet<WeighingMeasurementTrays>();
        }

        public long Id { get; set; }
        public long WeighingId { get; set; }
        public int ImageTypeId { get; set; }
        public int WeighStationTypeId { get; set; }
        public short Weight { get; set; }
        public string Notes { get; set; }
        public string Tiebreaker { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ImageTypes ImageType { get; set; }
        public WeighStationTypes WeighStationType { get; set; }
        public Weighings Weighing { get; set; }
        public ICollection<WeighingMeasurementImageMetadata> WeighingMeasurementImageMetadata { get; set; }
        public ICollection<WeighingMeasurementMenuItems> WeighingMeasurementMenuItems { get; set; }
        public ICollection<WeighingMeasurementTracking> WeighingMeasurementTracking { get; set; }
        public ICollection<WeighingMeasurementTrays> WeighingMeasurementTrays { get; set; }
    }
}
