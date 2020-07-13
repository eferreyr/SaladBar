using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class ViewTrayImageViewModel
    {
        public string SchoolName { get; set; }
        public DateTime MeasurementDate { get; set; }
        public List<Weighings> Weighings { get; set; }
    }
}
