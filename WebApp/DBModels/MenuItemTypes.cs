using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class MenuItemTypes
    {
        public MenuItemTypes()
        {
            MenuItems = new HashSet<MenuItems>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<MenuItems> MenuItems { get; set; }
    }
}
