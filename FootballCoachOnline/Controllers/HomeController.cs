using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballCoachOnline.Data;
using FootballCoachOnline.Models;
using FootballCoachOnline.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballCoachOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var userId = _userManager.GetUserId(User);
                var teams = _context.Team.Where(t => t.CoachId == userId).ToList();
                var competitions = _context.TeamCompetition
                    .Where(c => teams.Select(t => t.Id).Contains(c.TeamId))
                    .Include(c => c.Competition)
                    .Select(c => c.Competition)
                    .ToList();
                
                return View(new HomeViewModel{Competitions = competitions, Teams = teams});
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
