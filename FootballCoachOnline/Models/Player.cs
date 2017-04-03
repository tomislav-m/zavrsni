using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class Player
    {
        public Player()
        {
            MatchStats = new HashSet<MatchStats>();
            PlayerStats = new HashSet<PlayerStats>();
            PlayerTeam = new HashSet<PlayerTeam>();
            Test = new HashSet<Test>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nationality { get; set; }
        public string NaturalPosition { get; set; }
        public bool Physical { get; set; }
        public string Role { get; set; }

        public virtual ICollection<MatchStats> MatchStats { get; set; }
        public virtual ICollection<PlayerStats> PlayerStats { get; set; }
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
