using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class WeighingMeasurementMenuItemViewModel
    {
        public long Id { get; set; }
        public long WeighingMeasurementId { get; set; }
        public long MenuItemId { get; set; }
        public bool Selected { get; set; }
        public short? Quantity { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public MenuItemViewModel MenuItem { get; set; }
        public WeighingMeasurementViewModel WeighingMeasurement { get; set; }

        public WeighingMeasurementMenuItemViewModel() { }

        public WeighingMeasurementMenuItemViewModel(WeighingMeasurementMenuItems model)
        {
            this.Id = model.Id;
            this.WeighingMeasurementId = model.WeighingMeasurementId;
            this.MenuItemId = model.MenuItemId;
            this.Selected = model.Selected == "Y" ? true : false;
            this.Quantity = model.Quantity;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;

            this.MenuItem = model.MenuItem == null ? null : new MenuItemViewModel(model.MenuItem);
        }

        public WeighingMeasurementMenuItems ConvertToWeighingMeasurementMenuItems()
        {
            MenuItems menuItem = null;
            WeighingMeasurements weighingMeasurement = null;
            if (this.MenuItem != null) {
                menuItem = this.MenuItem.ConvertToMenuItems();
            }
            if (this.WeighingMeasurement != null)
            {
                weighingMeasurement = this.WeighingMeasurement.ConvertToWeighingMeasurements();
            }

            var weighingMeasurementMenuItem = new WeighingMeasurementMenuItems
            {
                Id = this.Id,
                WeighingMeasurementId = this.WeighingMeasurementId,
                MenuItemId = this.MenuItemId,
                Selected = this.Selected ? "Y" : "N",
                Quantity = this.Quantity,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy, 

                MenuItem = menuItem,
                WeighingMeasurement = weighingMeasurement
            };

            return weighingMeasurementMenuItem;
        }
    }
}
