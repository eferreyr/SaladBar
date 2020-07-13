using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models
{
    public class AllDataCollectionViewModel
    {
        public string SchoolName { get; set; }

        public DateTime DataColletionDate { get; set; }


        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true

        public AllDataCollectionViewModel() { }

        public AllDataCollectionViewModel(InterventionDays interventionDays) {
            SchoolName = interventionDays.School.Name;
            DataColletionDate = interventionDays.DtIntervention;
        }
    }
}
