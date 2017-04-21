using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class MatchScore
    {
        public int Id { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }

        public virtual Match Match { get; set; }
    }
}
