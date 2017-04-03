using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class TeamStats
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int CompetitionId { get; set; }
        public int Year { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Penalties { get; set; }
        public int PenaltiesConceded { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Team Team { get; set; }
    }
}
