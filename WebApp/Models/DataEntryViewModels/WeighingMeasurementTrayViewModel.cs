using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class WeighingMeasurementTrayViewModel
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public long InterventionDayTrayTypeId { get; set; }
        public short Quantity { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public InterventionDayTrayTypes InterventionDayTrayType { get; set; }
        public WeighingMeasurementViewModel WeighingMeasurement { get; set; }

        public WeighingMeasurementTrayViewModel() { }

        public WeighingMeasurementTrayViewModel(WeighingMeasurementTrays model)
        {
            this.Id = model.Id;
            this.WeighingMeasurementId = model.WeighingMeasurementId;
            this.InterventionDayTrayTypeId = model.InterventionDayTrayTypeId;
            this.Quantity = model.Quantity;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;

            this.InterventionDayTrayType = model.InterventionDayTrayType;
        }

        public WeighingMeasurementTrays ConvertToWeighingMeasurementTrays()
        {
            WeighingMeasurements weighingMeasurement = null;
            if (this.WeighingMeasurement != null)
            {
                weighingMeasurement = this.WeighingMeasurement.ConvertToWeighingMeasurements();
            }

            var weighingMeasurementTrays = new WeighingMeasurementTrays
            {
                Id = this.Id,
                WeighingMeasurementId = this.WeighingMeasurementId,
                InterventionDayTrayTypeId = this.InterventionDayTrayTypeId,
                Quantity = this.Quantity,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,

                InterventionDayTrayType = this.InterventionDayTrayType,
                WeighingMeasurement = weighingMeasurement
            };

            return weighingMeasurementTrays;
        }
    }
}
