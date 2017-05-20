using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{

    public partial class Player
    {
        public enum Position { golman, obrambeni, vezni, napadač };

        public Player()
        {
            MatchStats = new HashSet<MatchStats>();
            PlayerStats = new HashSet<PlayerStats>();
            PlayerTeam = new HashSet<PlayerTeam>();
            Test = new HashSet<Test>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }

        [Display(Name = "Ime")]
        public string Name { get; set; }

        [Display(Name = "Prezime")]
        public string Surname { get; set; }

        [Display(Name = "Datum rođenja")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Mjesto rođenja")]
        public string PlaceOfBirth { get; set; }

        [Display(Name = "Državljanstvo")]
        public string Nationality { get; set; }

        [Display(Name = "Pozicija")]
        public Position NaturalPosition { get; set; }

        [Display(Name = "Obavljen liječnički?")]
        public bool Physical { get; set; }
        public string Role { get; set; }

        public virtual ICollection<MatchStats> MatchStats { get; set; }
        public virtual ICollection<PlayerStats> PlayerStats { get; set; }
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }
        public virtual ICollection<Test> Test { get; set; }

        public string FullName {
            get
            {
                return Name + " " + Surname;
            }
        }
    }
}
