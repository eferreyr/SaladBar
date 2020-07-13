using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class MenuItems
    {
        public MenuItems()
        {
            WeighingMeasurementMenuItems = new HashSet<WeighingMeasurementMenuItems>();
        }

        public long Id { get; set; }
        public long MenuId { get; set; }
        public int MenuItemTypeId { get; set; }
        public string Name { get; set; }
        public string Quantifiable { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public Menus Menu { get; set; }
        public MenuItemTypes MenuItemType { get; set; }
        public ICollection<WeighingMeasurementMenuItems> WeighingMeasurementMenuItems { get; set; }
    }
}
