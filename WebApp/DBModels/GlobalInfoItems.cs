using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class GlobalInfoItems
    {
        public GlobalInfoItems()
        {
            WeighingMeasurmentGlobalInfoItems = new HashSet<WeighingMeasurmentGlobalInfoItems>();
        }

        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<WeighingMeasurmentGlobalInfoItems> WeighingMeasurmentGlobalInfoItems { get; set; }
    }
}
