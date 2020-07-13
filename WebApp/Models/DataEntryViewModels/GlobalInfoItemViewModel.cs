using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class GlobalInfoItemViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public DateTime DtCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DtModified { get; set; }
        public string ModifiedBy { get; set; }

        public GlobalInfoItemViewModel() { }

        public GlobalInfoItemViewModel(GlobalInfoItems model)
        {
            this.Id = model.Id;
            this.Type = model.Type;
            this.Name = model.Name;
            this.Active = model.Active;
            this.DtCreated = model.DtCreated;
            this.CreatedBy = model.CreatedBy;
            this.DtModified = model.DtModified;
            this.ModifiedBy = model.ModifiedBy;
        }

        public GlobalInfoItems ConvertToGlobalInfoItems()
        {
            var globalInfoItem = new GlobalInfoItems
            {
                Id = this.Id,
                Type = this.Type,
                Name = this.Name,
                Active = this.Active,
                DtCreated = this.DtCreated,
                CreatedBy = this.CreatedBy,
                DtModified = this.DtModified,
                ModifiedBy = this.ModifiedBy
            };

            return globalInfoItem;
        }
    }
}
