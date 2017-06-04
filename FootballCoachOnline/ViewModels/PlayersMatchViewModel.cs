using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FootballCoachOnline.ViewModels
{
    public class PlayersMatchViewModel
    {
        public SelectList PlayersSL { get; set; }
        public List<Player> Players { get; set; }
        public List<MatchStats> MatchStats { get; set; }
        public Match Match { get; set; }
    }
}
