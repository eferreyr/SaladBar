using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models
{
    public class SchoolViewModel
    {
        //public Schools()
        //{
        //    InterventionDays = new HashSet<InterventionDays>();
        //    Students = new HashSet<Students>();
        //}

        public long Id { get; set; }
        public string Name { get; set; }
        public string District { get; set; }
        public string Mascot { get; set; }
        public string Colors { get; set; }
        public byte[] SchoolLogo { get; set; }
        public int SchoolTypeId { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        //public SchoolTypes SchoolType { get; set; }
        //public ICollection<InterventionDays> InterventionDays { get; set; }
        //public ICollection<Students> Students { get; set; }


        public SchoolViewModel() { }

        public SchoolViewModel(Schools model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.District = model.District;
            this.Mascot = model.Mascot;
            this.Colors = model.Colors;
            this.SchoolLogo = model.SchoolLogo;
            this.SchoolTypeId = model.SchoolTypeId;
            this.Active = model.Active;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public Schools ConvertToSchools()
        {
            var school = new Schools
            {
                Id = this.Id,
                Name = this.Name,
                District = this.District,
                Mascot = this.Mascot,
                Colors = this.Colors,
                SchoolLogo = this.SchoolLogo,
                SchoolTypeId = this.SchoolTypeId,
                Active = this.Active,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy
            };

            return school;
        }
    }
}
