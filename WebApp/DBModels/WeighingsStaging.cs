using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class WeighingsStaging
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public string DeviceId { get; set; }
        public long WeighingId { get; set; }
        public long InterventionDayId { get; set; }
        public int WeighStationTypeId { get; set; }
        public byte[] Picture { get; set; }
        public string SaladDressing { get; set; }
        public string Milk { get; set; }
        public string UniqueSituation { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Empty { get; set; }
        public string Multiple { get; set; }
        public string Seconds { get; set; }
    }
}
