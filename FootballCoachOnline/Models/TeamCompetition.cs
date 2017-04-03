using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class TeamCompetition
    {
        public int TeamId { get; set; }
        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Team Team { get; set; }
    }
}
