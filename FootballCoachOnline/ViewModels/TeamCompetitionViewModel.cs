using System.Collections.Generic;
using FootballCoachOnline.Models;

namespace FootballCoachOnline.ViewModels
{
    public class TeamCompetitionViewModel
    {
        public Team Team { get; set; }
        public IEnumerable<Competition> Competitions { get; set; }
    }
}
