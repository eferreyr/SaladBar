using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class RandomizedStudents
    {
        public RandomizedStudents()
        {
            RandomizedStudentTrays = new HashSet<RandomizedStudentTrays>();
            WeighingMeasurmentGlobalInfoItems = new HashSet<WeighingMeasurmentGlobalInfoItems>();
        }

        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long InterventionDayId { get; set; }
        public string StudentId { get; set; }
        public string Assent { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public InterventionDays InterventionDay { get; set; }
        public Students S { get; set; }
        public ICollection<RandomizedStudentTrays> RandomizedStudentTrays { get; set; }
        public ICollection<WeighingMeasurmentGlobalInfoItems> WeighingMeasurmentGlobalInfoItems { get; set; }
    }
}
