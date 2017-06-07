using System.Collections.Generic;
using FootballCoachOnline.Models;

namespace FootballCoachOnline.ViewModels
{
    public class HomeViewModel
    {
        public List<Team> Teams { get; set; }
        public List<Competition> Competitions { get; set; }
    }
}