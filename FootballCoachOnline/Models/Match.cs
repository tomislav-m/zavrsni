using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{
    public partial class Match
    {
        public Match()
        {
            MatchStats = new HashSet<MatchStats>();
        }

        public int Id { get; set; }

        [Display(Name = "Natjecanje")]
        public int? CompetitionId { get; set; }

        public int MatchScoreId { get; set; }
        public virtual MatchScore MatchScore { get; set; }

        [Display(Name = "Domaćin")]
        public int Team1Id { get; set; }

        [Display(Name = "Gost")]
        public int Team2Id { get; set; }
        
        [Display(Name = "Datum"), DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ICollection<MatchStats> MatchStats { get; set; }

        [Display(Name = "Natjecanje")]
        public virtual Competition Competition { get; set; }

        [Display(Name = "Domaćin")]
        public virtual Team Team1 { get; set; }

        [Display(Name = "Gost")]
        public virtual Team Team2 { get; set; }
    }
}
