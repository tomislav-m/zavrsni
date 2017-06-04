using FootballCoachOnline.Models;
using System.Collections.Generic;

namespace FootballCoachOnline.ViewModels
{
    public class CompetitionStatsMatchesViewModel
    {
        public List<CompetitionTeamStats> Stats { get; set; }
        public List<List<Match>> Matches { get; set; }
        public List<PlayerStats> Scorers { get; set; }
        public List<PlayerStats> Yellows { get; set; }
        public List<PlayerStats> Reds { get; set; }
    }
}
