using FootballCoachOnline.Models;
using System.Collections.Generic;

namespace FootballCoachOnline.ViewModels
{
    public class PlayersMatchesViewModel
    {
        public List<Player> Players { get; set; }
        public List<Match> Matches { get; set; }
        public List<Training> Trainings { get; set; }
        public Team Team { get; set; }
    }
}
