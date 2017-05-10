using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class PlayersMatchViewModel
    {
        public SelectList PlayersSL { get; set; }
        public List<Player> Players { get; set; }
        public Match Match { get; set; }
    }
}
