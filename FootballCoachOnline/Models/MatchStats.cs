using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class MatchStats
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public bool App { get; set; }
        public bool Sub { get; set; }
        public int Goals { get; set; }
        public int? GoalsConceded { get; set; }
        public bool YellowCard { get; set; }
        public bool RedCard { get; set; }

        public virtual Match Match { get; set; }
        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
    }
}
