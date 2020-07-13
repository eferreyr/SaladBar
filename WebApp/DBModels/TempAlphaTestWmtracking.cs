using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class TempAlphaTestWmtracking
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Sequence { get; set; }
        public short Idx { get; set; }
        public string Wm { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
