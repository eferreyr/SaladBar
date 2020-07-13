using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class TrayTypeViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tray Name/Type")]
        public string Type { get; set; }

        [Display(Name = "Active?")]
        public bool Active { get; set; }

        public DateTime DtCreated { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Last Modified Time")]
        public DateTime? DtModified { get; set; }

        [Display(Name = "Last Modified By")]
        public string ModifiedBy { get; set; }

        public TrayTypeViewModel() { }

        public TrayTypeViewModel(TrayTypes model)
        {
            this.Id = model.Id;
            this.Type = model.Type;
            this.Active = model.Active == "Y" ? true : false;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public TrayTypes ConvertToTrayTypes()
        {
            var trayType = new TrayTypes
            {
                Id = this.Id,
                Type = this.Type,
                Active = this.Active ? "Y" : "N",
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy
            };

            return trayType;
        }
    }
}
