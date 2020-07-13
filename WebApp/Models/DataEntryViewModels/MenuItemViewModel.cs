using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class MenuItemViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long MenuId { get; set; }

        [Required]
        [Display(Name = "Item Type")]
        public int MenuItemTypeId { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [Display(Name = "Quantifiable?")]
        public bool Quantifiable { get; set; }

        [Display(Name = "Active?")]
        public bool Active { get; set; }

        public DateTime DtCreated { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Last Modified Time")]
        public DateTime? DtModified { get; set; }

        [Display(Name = "Last Modified By")]
        public string ModifiedBy { get; set; }

        //public Menus Menu { get; set; }

        public MenuItemTypes MenuItemType { get; set; }

        //public ICollection<WeighingMeasurementMenuItems> WeighingMeasurementMenuItems { get; set; }

        public MenuItemViewModel() { }

        public MenuItemViewModel(MenuItems model)
        {
            this.Id = model.Id;
            this.MenuId = model.MenuId;
            this.MenuItemTypeId = model.MenuItemTypeId;
            this.Name = model.Name;
            this.Quantifiable = model.Quantifiable == "Y" ? true : false;
            this.Active = model.Active == "Y" ? true : false;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
            this.MenuItemType = model.MenuItemType;
        }

        public MenuItems ConvertToMenuItems()
        {
            var menuItem = new MenuItems
            {
                Id = this.Id,
                MenuId = this.MenuId,
                MenuItemTypeId = this.MenuItemTypeId,
                Name = this.Name,
                Quantifiable = this.Quantifiable ? "Y" : "N",
                Active = this.Active ? "Y" : "N",
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,
                MenuItemType = this.MenuItemType
            };

            return menuItem;
        }
    }
}
