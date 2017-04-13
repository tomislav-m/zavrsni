using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class CompetitionTeamStatsViewModel
    {
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Points { get; set; }
    }
}
