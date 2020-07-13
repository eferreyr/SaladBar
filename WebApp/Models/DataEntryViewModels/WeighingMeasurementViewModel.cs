using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class WeighingMeasurementViewModel
    {
        public long Id { get; set; }
        public long WeighingId { get; set; }
        public int ImageTypeId { get; set; }
        public int WeighStationTypeId { get; set; }
        public short Weight { get; set; } = -1;
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
        public List<WeighingMeasurementImageMetadataViewModel> WeighingMeasurementImageMetadata { get; set; }
        public List<WeighingMeasurementMenuItemViewModel> WeighingMeasurementMenuItems { get; set; }
        public List<WeighingMeasurementTrackingViewModel> WeighingMeasurementTracking { get; set; }
        public List<WeighingMeasurementTrayViewModel> WeighingMeasurementTrays { get; set; }

        public WeighingMeasurementViewModel() { }

        public WeighingMeasurementViewModel(WeighingMeasurements model)
        {
            this.Id = model.Id;
            this.WeighingId = model.WeighingId;
            this.ImageTypeId = model.ImageTypeId;
            this.WeighStationTypeId = model.WeighStationTypeId;
            this.Weight = model.Weight;
            this.Notes = model.Notes;
            this.Tiebreaker = model.Tiebreaker;
            this.Active = model.Active;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;

            this.ImageType = model.ImageType;
            this.WeighStationType = model.WeighStationType;
            this.Weighing = model.Weighing;
            this.WeighingMeasurementImageMetadata = model.WeighingMeasurementImageMetadata
                .Select(x => new WeighingMeasurementImageMetadataViewModel(x))
                .ToList();
            this.WeighingMeasurementMenuItems = model.WeighingMeasurementMenuItems
                .Select(x => new WeighingMeasurementMenuItemViewModel(x))
                .ToList();
            this.WeighingMeasurementTracking = model.WeighingMeasurementTracking
                .Select(x => new WeighingMeasurementTrackingViewModel(x))
                .ToList();
            this.WeighingMeasurementTrays = model.WeighingMeasurementTrays
                .Select(x => new WeighingMeasurementTrayViewModel(x))
                .ToList();
        }

        public WeighingMeasurements ConvertToWeighingMeasurements()
        {
            var weighingMeasurements = new WeighingMeasurements
            {
                Id = this.Id,
                WeighingId = this.WeighingId,
                ImageTypeId = this.ImageTypeId,
                WeighStationTypeId = this.WeighStationTypeId,
                Weight = this.Weight,
                Notes = this.Notes,
                Tiebreaker = this.Tiebreaker,
                Active = this.Active,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,

                ImageType = this.ImageType,
                WeighStationType = this.WeighStationType,
                Weighing = this.Weighing,
                WeighingMeasurementImageMetadata = this.WeighingMeasurementImageMetadata
                    .Select(x => x.ConvertToWeighingMeasurementImageMetadata())
                    .ToList(),
                WeighingMeasurementMenuItems = this.WeighingMeasurementMenuItems
                    .Select(x => x.ConvertToWeighingMeasurementMenuItems())
                    .ToList(),
                WeighingMeasurementTracking = this.WeighingMeasurementTracking
                    .Select(x => x.ConvertToWeighingMeasurementTracking())
                    .ToList(),
                WeighingMeasurementTrays = this.WeighingMeasurementTrays
                    .Select(x => x.ConvertToWeighingMeasurementTrays())
                    .ToList()
            };

            return weighingMeasurements;
        }
    }
}
