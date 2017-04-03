using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class PlayerStats
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public DateTime Year { get; set; }
        public int Apps { get; set; }
        public int Subs { get; set; }
        public int Goals { get; set; }
        public int GoalsConceded { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }

        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
    }
}
