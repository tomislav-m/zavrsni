using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{
    public partial class Club
    {
        public Club()
        {
            Team = new HashSet<Team>();
        }

        public int Id { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Unos imena je obavezan")]
        [StringLength(40, ErrorMessage = "Dužina imena smije biti najviše 40 znakova")]
        public string Name { get; set; }

        [Display(Name = "Nadimak")]
        [StringLength(20, ErrorMessage = "Dužina nadimka smije biti najviše 20 znakova")]
        public string Nickname { get; set; }

        [Display(Name = "Godina osnutka")]
        [Range(1850, 2017, ErrorMessage = "Unesite valjanu godinu")]
        public int? YearFounded { get; set; }

        public virtual ICollection<Team> Team { get; set; }
    }
}
