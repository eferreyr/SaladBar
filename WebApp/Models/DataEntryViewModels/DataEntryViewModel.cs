using Microsoft.AspNetCore.Mvc;
using SaladBarWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    [BindProperties]
    public class DataEntryViewModel
    {
        public List<WeighingMeasurementViewModel> WeighingMeasurements { get; set; }
        public List<WeighingMeasurementGlobalInfoItemViewModel> WeighingMeasurementGlobalInfoItems { get; set; }


        public List<Weighings> Weighings { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; }
        public List<MenuItemTypeViewModel> MenuItemTypes { get; set; }
        public List<ImageMetadataViewModel> ImageMetadata { get; set; }
    }
}
