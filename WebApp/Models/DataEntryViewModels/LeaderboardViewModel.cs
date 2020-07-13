using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaladBarWeb.DBModels;

namespace SaladBarWeb.Models.DataEntryViewModels
{
    public class LeaderboardViewModel
    {
        public int Count { get; set; }
        public ResearchTeamMembers ResearchTeamMember { get; set; }

        public LeaderboardViewModel() { }
    }
}
