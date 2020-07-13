using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class WeighingMeasurementGlobalInfoItemViewModel
    {
        public long Id { get; set; }
        public long RandomizedStudentId { get; set; }
        public long GlobalInfoItemId { get; set; }
        public string Value { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public GlobalInfoItemViewModel GlobalInfoItem { get; set; }

        public WeighingMeasurementGlobalInfoItemViewModel() { }

        public WeighingMeasurementGlobalInfoItemViewModel(WeighingMeasurmentGlobalInfoItems model)
        {
            this.Id = model.Id;
            this.RandomizedStudentId = model.RandomizedStudentId;
            this.GlobalInfoItemId = model.GlobalInfoItemId;
            this.Value = model.Value;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;

            this.GlobalInfoItem = model.GlobalInfoItem == null ? null : new GlobalInfoItemViewModel(model.GlobalInfoItem);
        }

        public WeighingMeasurmentGlobalInfoItems ConvertToWeighingMeasurmentGlobalInfoItems()
        {
            GlobalInfoItems globalInfoItem = null;
            if (this.GlobalInfoItem != null)
            {
                globalInfoItem = this.GlobalInfoItem.ConvertToGlobalInfoItems();
            }

            var weighingMeasurmentGlobalInfoItem = new WeighingMeasurmentGlobalInfoItems
            {
                Id = this.Id,
                RandomizedStudentId = this.RandomizedStudentId,
                GlobalInfoItemId = this.GlobalInfoItemId,
                Value = this.Value,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,

                GlobalInfoItem = globalInfoItem
            };

            return weighingMeasurmentGlobalInfoItem;
        }
    }
}
