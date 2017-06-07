using FootballCoachOnline.Models;
using System.Collections.Generic;

namespace FootballCoachOnline.ViewModels
{
    public class PlayersMatchesViewModel
    {
        public List<Match> Matches { get; set; }
        public List<Training> Trainings { get; set; }
        public Team Team { get; set; }
        public Dictionary<int, PlayerStats> PlayerStats { get; set; }
    }
}
