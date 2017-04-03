using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballCoachOnline.Models;

namespace FootballCoachOnline.ViewModels
{
    public class TeamCompetitionViewModel
    {
        public Team Team { get; set; }
        public IEnumerable<Competition> Competitions { get; set; }
    }
}
