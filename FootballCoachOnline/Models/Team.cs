using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FootballCoachOnline.Models
{
    public partial class Team
    {
        public Team()
        {
            MatchTeam1 = new HashSet<Match>();
            MatchTeam2 = new HashSet<Match>();
            PlayerStats = new HashSet<PlayerStats>();
            PlayerTeam = new HashSet<PlayerTeam>();
            TeamCompetition = new HashSet<TeamCompetition>();
            TeamStats = new HashSet<TeamStats>();
            Training = new HashSet<Training>();
        }

        public int Id { get; set; }
        public string CoachId { get; set; }
        public int ClubId { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Unos imena je obavezan")]
        [StringLength(40, ErrorMessage = "Dužina imena smije biti najviše 40 znakova")]
        public string Name { get; set; }

        [Display(Name = "Uzrast")]
        [Required(ErrorMessage = "Unos uzrasta je obavezan")]
        [StringLength(15, ErrorMessage = "Dužina naziva uzrasta smije biti najviše 15 znakova")]
        public string Age { get; set; }

        [Display(Name = "Skraćenica")]
        [Required(ErrorMessage = "Unos skraćenice je obavezan")]
        [StringLength(15, ErrorMessage = "Dužina skraćenice smije biti najviše 15 znakova")]
        public string ShortName { get; set; }

        public virtual ICollection<Match> MatchTeam1 { get; set; }
        public virtual ICollection<Match> MatchTeam2 { get; set; }
        public virtual ICollection<PlayerStats> PlayerStats { get; set; }
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }
        public virtual ICollection<TeamCompetition> TeamCompetition { get; set; }
        public virtual ICollection<TeamStats> TeamStats { get; set; }
        public virtual ICollection<Training> Training { get; set; }

        [Display(Name = "Klub")]
        public virtual Club Club { get; set; }
    }
}
