using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{
    public partial class Competition
    {
        public Competition()
        {
            Match = new HashSet<Match>();
            TeamCompetition = new HashSet<TeamCompetition>();
            TeamStats = new HashSet<TeamStats>();
        }

        public int Id { get; set; }

        [Display(Name = "Ime")]
        public string Name { get; set; }

        [Display(Name = "Skraćenica")]
        public string ShortName { get; set; }

        public virtual ICollection<Match> Match { get; set; }
        public virtual ICollection<TeamCompetition> TeamCompetition { get; set; }
        public virtual ICollection<TeamStats> TeamStats { get; set; }
    }
}
