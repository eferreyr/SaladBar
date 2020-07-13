using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class ImageMetadataViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public List<WeighingMeasurementImageMetadataViewModel> WeighingMeasurementImageMetadata { get; set; }

        public ImageMetadataViewModel() { }

        public ImageMetadataViewModel(ImageMetadata model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Active = model.Active;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public ImageMetadata ConvertToImageMetadata()
        {
            var imageMetadata = new ImageMetadata
            {
                Id = this.Id,
                Name = this.Name,
                Active = this.Active,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,

                WeighingMeasurementImageMetadata = this.WeighingMeasurementImageMetadata
                    .Select(x => x.ConvertToWeighingMeasurementImageMetadata())
                    .ToList()
            };

            return imageMetadata;
        }
    }
}
