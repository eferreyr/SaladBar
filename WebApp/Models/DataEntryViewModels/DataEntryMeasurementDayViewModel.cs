using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaladBarWeb.DBModels;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class DataEntryMeasurementDayViewModel
    {
        public InterventionDays InterventionDay { get; set; }
        public DataEntryLocks DataEntryLock { get; set; }
        public ResearchTeamMembers LockedUser { get; set; }
        public int TotalWeighings { get; set; }
        public int TotalCompletedFirstDataEntry { get; set; }
        public int TotalCompletedSecondDataEntry { get; set; }
        public int TotalCompletedThirdDataEntry { get; set; }

        public DataEntryMeasurementDayViewModel() { }

        public DataEntryMeasurementDayViewModel(InterventionDays interventionDay, DataEntryLocks dataEntryLock, ResearchTeamMembers lockedUser)
        {
            this.InterventionDay = interventionDay;
            this.DataEntryLock = dataEntryLock;
            this.LockedUser = lockedUser;
        }
    }
}
