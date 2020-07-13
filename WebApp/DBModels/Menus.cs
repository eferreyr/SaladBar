using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class Menus
    {
        public Menus()
        {
            MenuItems = new HashSet<MenuItems>();
        }

        public long Id { get; set; }
        public long InterventionDayId { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public InterventionDays InterventionDay { get; set; }
        public ICollection<MenuItems> MenuItems { get; set; }
    }
}
