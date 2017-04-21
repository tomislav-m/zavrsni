using FootballCoachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class CompetitionStatsMatchesViewModel
    {
        public List<CompetitionTeamStats> Stats { get; set; }
        public List<List<Match>> Matches { get; set; }
    }
}
