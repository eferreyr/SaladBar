using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingMeasurmentGlobalInfoItems
    {
        public long Id { get; set; }
        public long RandomizedStudentId { get; set; }
        public long GlobalInfoItemId { get; set; }
        public string Value { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public GlobalInfoItems GlobalInfoItem { get; set; }
        public RandomizedStudents RandomizedStudent { get; set; }
    }
}
