using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class DataEntryLocks
    {
        public long Id { get; set; }
        public string AspNetUserId { get; set; }
        public long InterventionDayId { get; set; }
        public string Locked { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public AspNetUsers AspNetUser { get; set; }
        public InterventionDays InterventionDay { get; set; }
    }
}
