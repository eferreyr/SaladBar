using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class WeighingMeasurementImageMetadataViewModel
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public int ImageMetadataId { get; set; }
        public bool Selected { get; set; }
        public string Value { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ImageMetadataViewModel ImageMetadata { get; set; }
        public WeighingMeasurementViewModel WeighingMeasurement { get; set; }

        public WeighingMeasurementImageMetadataViewModel() { }

        public WeighingMeasurementImageMetadataViewModel(WeighingMeasurementImageMetadata model)
        {
            this.Id = model.Id;
            this.WeighingMeasurementId = model.WeighingMeasurementId;
            this.ImageMetadataId = model.ImageMetadataId;
            this.Selected = model.Selected == "Y" ? true : false;
            this.Value = model.Value.ToString();
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;

            this.ImageMetadata = model.ImageMetadata == null ? null : new ImageMetadataViewModel(model.ImageMetadata);
        }

        public WeighingMeasurementImageMetadata ConvertToWeighingMeasurementImageMetadata()
        {
            ImageMetadata imageMetadata = null;
            WeighingMeasurements weighingMeasurement = null;
            if (this.ImageMetadata != null)
            {
                imageMetadata = this.ImageMetadata.ConvertToImageMetadata();
            }
            if (this.WeighingMeasurement != null)
            {
                weighingMeasurement = this.WeighingMeasurement.ConvertToWeighingMeasurements();
            }

            var weighingMeasurementImageMetadata = new WeighingMeasurementImageMetadata
            {
                Id = this.Id,
                WeighingMeasurementId = this.WeighingMeasurementId,
                ImageMetadataId = this.ImageMetadataId,
                Selected = this.Selected ? "Y" : "N",
                Value = this.Value == null ? Convert.ToInt16(0) : Convert.ToInt16(this.Value),
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,

                ImageMetadata = imageMetadata,
                WeighingMeasurement = weighingMeasurement
            };

            return weighingMeasurementImageMetadata;
        }
    }
}
