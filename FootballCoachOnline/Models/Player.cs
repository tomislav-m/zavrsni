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
        [Required(ErrorMessage = "Unos imena je obavezan")]
        [StringLength(40, ErrorMessage = "Dužina imena smije biti najviše 40 znakova")]
        public string Name { get; set; }

        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Unos prezimena je obavezan")]
        [StringLength(40, ErrorMessage = "Dužina prezimena smije biti najviše 40 znakova")]
        public string Surname { get; set; }

        [Display(Name = "Datum rođenja")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Mjesto rođenja")]
        [StringLength(30, ErrorMessage = "Dužina mjesta rođenja smije biti najviše 30 znakova")]
        public string PlaceOfBirth { get; set; }

        [Display(Name = "Državljanstvo")]
        [StringLength(20, ErrorMessage = "Dužina državljanstva smije biti najviše 20 znakova")]
        public string Nationality { get; set; }

        [Display(Name = "Pozicija")]
        [Required(ErrorMessage = "Odabir pozicije je obavezan")]
        public Position NaturalPosition { get; set; }

        [Display(Name = "Obavljen liječnički?")]
        public bool Physical { get; set; }

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
