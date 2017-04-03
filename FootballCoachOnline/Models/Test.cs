using System;
using System.Collections.Generic;

namespace FootballCoachOnline.Models
{
    public partial class Test
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Player Player { get; set; }
    }
}
