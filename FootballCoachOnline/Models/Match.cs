using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class Match
    {
        public Match()
        {
            MatchScore = new HashSet<MatchScore>();
            MatchStats = new HashSet<MatchStats>();
        }

        public int Id { get; set; }
        public int? CompetitionId { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<MatchScore> MatchScore { get; set; }
        public virtual ICollection<MatchStats> MatchStats { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
    }
}
