using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class MeasurementViewModel
    {
        public long Id { get; set; }
        public long WeighingId { get; set; }
        public int ImageTypeId { get; set; }
        public int WeighStationTypeId { get; set; }
        public short Weight { get; set; }
        public string Notes { get; set; }
        public string Tiebreaker { get; set; }

        public ImageTypeViewModel ImageType { get; set; }
        public WeighStationTypes WeighStationType { get; set; }
        public Weighings Weighing { get; set; }
        public ICollection<WeighingMeasurementImageMetadata> WeighingMeasurementImageMetadata { get; set; }
        public ICollection<WeighingMeasurementMenuItems> WeighingMeasurementMenuItems { get; set; }
        public ICollection<WeighingMeasurementTrays> WeighingMeasurementTrays { get; set; }

        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }


    }
}
