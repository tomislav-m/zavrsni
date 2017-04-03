using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class CompetitionViewModel
    {
        public Team Team { get; set; }
        public SelectList CompetitionSL { get; set; }
        public int CompetitionId { get; set; }
    }
}
