using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class Training
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public virtual Team Team { get; set; }
    }
}
