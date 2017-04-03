using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class PlayerTeam
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }

        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
    }
}
