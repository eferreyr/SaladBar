using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models
{
    public class InterventionDayViewModel
    {
        public long Id { get; set; }

        public long SchoolId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DtIntervention { get; set; }

        public int SampleSize { get; set; }

        public string InterventionFinished { get; set; }

        public string Active { get; set; }

        public DateTime DtCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DtModified { get; set; }

        public string ModifiedBy { get; set; }

        public SchoolViewModel School { get; set; }
        //public ICollection<InterventionDayTrayTypes> InterventionDayTrayTypes { get; set; }
        //public ICollection<InterventionTrays> InterventionTrays { get; set; }
        //public ICollection<Menus> Menus { get; set; }
        //public ICollection<RandomizedStudents> RandomizedStudents { get; set; }
        //public ICollection<Weighings> Weighings { get; set; }


        public InterventionDayViewModel() { }

        public InterventionDayViewModel(InterventionDays model)
        {
            this.Id = model.Id;
            this.School = new SchoolViewModel(model.School);
            this.DtIntervention = model.DtIntervention;
            this.SampleSize = model.SampleSize;
            this.InterventionFinished = model.InterventionFinished;
            this.Active = model.Active;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public InterventionDays ConvertToInterventionDays()
        {
            var interventionDay = new InterventionDays
            {
                Id = this.Id,
                School = this.School.ConvertToSchools(),
                DtIntervention = this.DtIntervention,
                SampleSize = this.SampleSize,
                InterventionFinished = this.InterventionFinished,
                Active = this.Active,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy
            };

            return interventionDay;
        }
    }
}
