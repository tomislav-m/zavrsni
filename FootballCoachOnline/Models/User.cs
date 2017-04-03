using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class User
    {
        public User()
        {
            Team = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Team> Team { get; set; }
    }
}
