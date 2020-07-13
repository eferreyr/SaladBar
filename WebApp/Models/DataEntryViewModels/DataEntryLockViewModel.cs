using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaladBarWeb.DBModels;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class DataEntryLockViewModel
    {
        public long dataEntryLockId { get; set; }
        public ResearchTeamMembers LockedUser { get; set; }
        public InterventionDays InterventionDay { get; set; }
        public bool LockStatus { get; set; }

        public DataEntryLockViewModel() { }

        public DataEntryLockViewModel(DataEntryLocks dataEntryLock, ResearchTeamMembers lockedUser)
        {
            this.dataEntryLockId = dataEntryLock.Id;
            this.LockedUser = lockedUser;
            this.InterventionDay = dataEntryLock.InterventionDay;
            this.LockStatus = dataEntryLock.Locked == "Y" ? true : false;
        }
    }
}
