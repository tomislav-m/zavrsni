using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballCoachOnline.ViewModels
{
    public class CompetitionViewModel
    {
        public Team Team { get; set; }
        public SelectList CompetitionSL { get; set; }
        public int CompetitionId { get; set; }
    }
}
