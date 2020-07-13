using SaladBarWeb.DBModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models
{
    public class TestViewModel
    {
        public string SchoolName { get; set; }

        public DateTime DataColletionDate { get; set; }

        public string SchoolType { get; set; }

        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true

        public TestViewModel() { }

        public TestViewModel(InterventionDays interventionDays)
        {
            SchoolName = interventionDays.School.Name;
            DataColletionDate = interventionDays.DtIntervention;
            SchoolType = interventionDays.School.SchoolType.Type;
        }
    }
}