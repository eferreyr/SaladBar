using System;
using System.Collections.Generic;

namespace SaladBarWeb.DBModels
{
    public partial class Weighings
    {
        public Weighings()
        {
            WeighingMeasurements = new HashSet<WeighingMeasurements>();
            WeighingTrays = new HashSet<WeighingTrays>();
        }

        public long Id { get; set; }
        public long InterventionDayId { get; set; }
        public int WeighStationTypeId { get; set; }
        public byte[] Picture { get; set; }
        public string SaladDressing { get; set; }
        public string Milk { get; set; }
        public string UniqueSituation { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Empty { get; set; }
        public string Multiple { get; set; }
        public string Seconds { get; set; }

        public InterventionDays InterventionDay { get; set; }
        public WeighStationTypes WeighStationType { get; set; }
        public ICollection<WeighingMeasurements> WeighingMeasurements { get; set; }
        public ICollection<WeighingTrays> WeighingTrays { get; set; }
    }
}
