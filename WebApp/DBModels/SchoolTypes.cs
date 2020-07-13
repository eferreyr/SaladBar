using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class SchoolTypes
    {
        public SchoolTypes()
        {
            Schools = new HashSet<Schools>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Grades { get; set; }
        public int RandomSampleSize { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Schools> Schools { get; set; }
    }
}
