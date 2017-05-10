using FootballCoachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class PlayersMatchesViewModel
    {
        public List<Player> Players { get; set; }
        public List<Match> Matches { get; set; }
        public Team Team { get; set; }
    }
}
