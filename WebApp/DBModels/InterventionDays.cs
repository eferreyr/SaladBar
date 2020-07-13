using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class InterventionDays
    {
        public InterventionDays()
        {
            DataEntryLocks = new HashSet<DataEntryLocks>();
            InterventionDayTrayTypes = new HashSet<InterventionDayTrayTypes>();
            InterventionTrays = new HashSet<InterventionTrays>();
            Menus = new HashSet<Menus>();
            RandomizedStudents = new HashSet<RandomizedStudents>();
            Weighings = new HashSet<Weighings>();
        }

        public long Id { get; set; }
        public long SchoolId { get; set; }
        public DateTime DtIntervention { get; set; }
        public int SampleSize { get; set; }
        public string InterventionFinished { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public Schools School { get; set; }
        public ICollection<DataEntryLocks> DataEntryLocks { get; set; }
        public ICollection<InterventionDayTrayTypes> InterventionDayTrayTypes { get; set; }
        public ICollection<InterventionTrays> InterventionTrays { get; set; }
        public ICollection<Menus> Menus { get; set; }
        public ICollection<RandomizedStudents> RandomizedStudents { get; set; }
        public ICollection<Weighings> Weighings { get; set; }
    }
}
