﻿using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class RandomizedStudentsStaging
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public string DeviceId { get; set; }
        public long RandomizedStudentId { get; set; }
        public long InterventionDayId { get; set; }
        public string StudentId { get; set; }
        public string Assent { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
