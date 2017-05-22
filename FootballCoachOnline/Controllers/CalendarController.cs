using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballCoachOnline.Data;
using Microsoft.AspNetCore.Identity;
using FootballCoachOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballCoachOnline.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CalendarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetEvents()
        {

            if (_signInManager.IsSignedIn(User))
            {
                var teams = _context.Team
                                    .Where(t => t.CoachId == _userManager.GetUserId(User))
                                    .Include(t => t.MatchTeam1)
                                    .ThenInclude(t => t.Team2)
                                    .Include(t => t.MatchTeam1)
                                    .ThenInclude(t => t.MatchScore)
                                    .Include(t => t.MatchTeam2)
                                    .ThenInclude(t => t.Team1)
                                    .Include(t => t.MatchTeam2)
                                    .ThenInclude(t => t.MatchScore)
                                    .Include(t => t.Training);

                List<object> events = new List<object>();
                foreach (var item in teams)
                {
                    var matches = item.MatchTeam1.ToList();
                    matches.AddRange(item.MatchTeam2);
                    foreach (var match in matches)
                    {
                        var score = " - ";
                        if (match.Played)
                        {
                            score = " " + match.MatchScore.Score1.ToString() + " : " + match.MatchScore.Score2.ToString() + " ";
                        }
                        var result = new
                        {
                            title = "Utakmica\n" + match.Team1.ShortName + score + match.Team2.ShortName,
                            start = match.Date,
                            end = match.Date.AddMinutes(105),
                            url = Url.Action("Details", "Matches", new { id = match.Id })
                        };
                        events.Add(result);
                    }
                    var trainings = item.Training.ToList();
                    foreach (var training in trainings)
                    {
                        var result = new
                        {
                            title = "Trening\n" + item.ShortName,
                            start = training.Date,
                            end = training.Date,
                            url = Url.Action("Details", "Trainings", new { id = training.Id }),
                            color = "green"
                        };
                        events.Add(result);
                    }
                }
                return Json(events);
            }
            return null;
        }
    }
}