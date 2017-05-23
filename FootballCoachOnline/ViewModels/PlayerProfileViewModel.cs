using FootballCoachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class PlayerProfileViewModel
    {
        public Player Player { get; set; }
        public PlayerStats Stats { get; set; }
    }
}
