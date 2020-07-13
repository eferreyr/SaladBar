using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class Schools
    {
        public Schools()
        {
            InterventionDays = new HashSet<InterventionDays>();
            Students = new HashSet<Students>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string District { get; set; }
        public string Mascot { get; set; }
        public string Colors { get; set; }
        public byte[] SchoolLogo { get; set; }
        public int SchoolTypeId { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public SchoolTypes SchoolType { get; set; }
        public ICollection<InterventionDays> InterventionDays { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}
