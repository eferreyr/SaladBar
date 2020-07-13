using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class Students
    {
        public Students()
        {
            RandomizedStudents = new HashSet<RandomizedStudents>();
        }

        public long Id { get; set; }
        public long SchoolId { get; set; }
        public string StudentId { get; set; }
        public string Gender { get; set; }
        public int? Grade { get; set; }
        public int? Age { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string PaidFreeReduced { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public Schools School { get; set; }
        public ICollection<RandomizedStudents> RandomizedStudents { get; set; }
    }
}
