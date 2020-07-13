using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class InterventionDayTrayTypeViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Tray Type")]
        public int TrayTypeId { get; set; }

        [Required]
        [Display(Name = "Wave Date")]
        public long InterventionDayId { get; set; }

        [Display(Name = "Active?")]
        public bool Active { get; set; }

        public DateTime DtCreated { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Last Modified Time")]
        public DateTime? DtModified { get; set; }

        [Display(Name = "Last Modified By")]
        public string ModifiedBy { get; set; }

        public InterventionDays InterventionDay { get; set; }

        public TrayTypes TrayType { get; set; }

        public InterventionDayTrayTypeViewModel() { }

        public InterventionDayTrayTypeViewModel(InterventionDayTrayTypes model)
        {
            this.Id = model.Id;
            this.TrayTypeId = model.TrayTypeId;
            this.InterventionDayId = model.InterventionDayId;
            this.Active = model.Active == "Y" ? true : false;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
            this.InterventionDay = model.InterventionDay;
            this.TrayType = model.TrayType;
        }

        public InterventionDayTrayTypes ConvertToInterventionDayTrayTypes()
        {
            var interventionDayTrayType = new InterventionDayTrayTypes
            {
                Id = this.Id,
                TrayTypeId = this.TrayTypeId,
                InterventionDayId = this.InterventionDayId,
                Active = this.Active ? "Y" : "N",
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy,
                InterventionDay = this.InterventionDay,
                TrayType = this.TrayType
            };

            return interventionDayTrayType;
        }
    }
}
