using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class WeighingMeasurementTrackingViewModel
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public string Info { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public WeighingMeasurementViewModel WeighingMeasurement { get; set; }

        public WeighingMeasurementTrackingViewModel() { }

        public WeighingMeasurementTrackingViewModel(WeighingMeasurementTracking model)
        {
            this.Id = model.Id;
            this.WeighingMeasurementId = model.WeighingMeasurementId;
            this.Info = model.Info;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public WeighingMeasurementTracking ConvertToWeighingMeasurementTracking()
        {
            WeighingMeasurements weighingMeasurement = null;
            if (this.WeighingMeasurement != null)
            {
                weighingMeasurement = this.WeighingMeasurement.ConvertToWeighingMeasurements();
            }

            var weighingMeasurementTracking = new WeighingMeasurementTracking
            {
                Id = this.Id,
                WeighingMeasurementId = this.WeighingMeasurementId,
                Info = this.Info,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,
                
                WeighingMeasurement = weighingMeasurement
            };

            return weighingMeasurementTracking;
        }
    }
}
