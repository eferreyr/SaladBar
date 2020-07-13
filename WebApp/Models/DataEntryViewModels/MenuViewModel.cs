using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class MenuViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Measurement Date")]
        public long InterventionDayId { get; set; }

        [Display(Name = "Menu Name")]
        public string Name { get; set; }

        [Display(Name = "Active?")]
        public bool Active { get; set; }

        public DateTime DtCreated { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Last Modified Time")]
        public DateTime? DtModified { get; set; }

        [Display(Name = "Last Modified By")]
        public string ModifiedBy { get; set; }

        public InterventionDays InterventionDay { get; set; }

        public ICollection<MenuItems> MenuItems { get; set; }

        public MenuViewModel() { }

        public MenuViewModel(Menus model)
        {
            this.Id = model.Id;
            this.InterventionDayId = model.InterventionDayId;
            this.Name = model.Name;
            this.Active = model.Active == "Y" ? true : false;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
            this.InterventionDay = model.InterventionDay;
            this.MenuItems = model.MenuItems;
        }

        public Menus ConvertToMenus()
        {
            var menu = new Menus
            {
                Id = this.Id,
                InterventionDayId = this.InterventionDayId,
                Name = this.Name,
                Active = this.Active ? "Y" : "N",
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,
                InterventionDay = this.InterventionDay,
                MenuItems = this.MenuItems
            };

            return menu;
        }
    }
}
