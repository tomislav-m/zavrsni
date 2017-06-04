using FootballCoachOnline.Models;
using System.Collections.Generic;

namespace FootballCoachOnline.ViewModels
{
    public class PlayerProfileViewModel
    {
        public Player Player { get; set; }
        public List<PlayerStats> Stats { get; set; }
        public List<Test> Tests { get; set; }
    }
}
