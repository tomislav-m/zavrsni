using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{
    public partial class PlayerStats
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        [Display(Name = "Tim")]
        public int TeamId { get; set; }
        public DateTime Year { get; set; }

        [Display(Name = "Nastupi")]
        public int Apps { get; set; }
        public int Subs { get; set; }

        [Display(Name = "Golovi")]
        public int Goals { get; set; }

        [Display(Name = "Primljeni golovi")]
        public int GoalsConceded { get; set; }

        [Display(Name = "Žuti kartoni")]
        public int YellowCards { get; set; }

        [Display(Name = "Crveni kartoni")]
        public int RedCards { get; set; }

        public virtual Player Player { get; set; }

        [Display(Name = "Tim")]
        public virtual Team Team { get; set; }
    }
}
