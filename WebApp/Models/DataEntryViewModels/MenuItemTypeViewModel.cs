using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class MenuItemTypeViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Active { get; set; }

        public DateTime DtCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DtModified { get; set; }

        public string ModifiedBy { get; set; }

        //public ICollection<Men> MenuItems { get; set; }

        public MenuItemTypeViewModel() { }

        public MenuItemTypeViewModel(MenuItemTypes model)
        {
            this.Id = model.Id;
            this.Type = model.Type;
            this.Active = model.Active;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public MenuItemTypes ConvertToMenus()
        {
            var menuItemTypes = new MenuItemTypes
            {
                Id = this.Id,
                Type = this.Type,
                Active = this.Active,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy
            };

            return menuItemTypes;
        }
    }
}
