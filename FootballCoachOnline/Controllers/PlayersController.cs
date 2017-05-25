using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Data;
using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Identity;
using FootballCoachOnline.ViewModels;

namespace FootballCoachOnline.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            return View(await _context.Player.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id, int? year)
        {
            if (id == null)
            {
                return NotFound();
            }
            if(year == null)
            {
                year = DateTime.Now.Year;
            }

            var player = await _context.Player.SingleOrDefaultAsync(m => m.Id == id);
            var stats = _context.PlayerStats
                .Where(s => s.PlayerId == id && s.Year.Year == year)
                .Include(s => s.Team)
                .Include(s => s.Competition)
                .OrderBy(s => s.Competition.Name)
                .ToList();

            if (player == null)
            {
                return NotFound();
            }
            var vm = new PlayerProfileViewModel
            {
                Player = player,
                Stats = stats
            };

            return View(vm);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            var teams = _context.Team.Where(t => t.CoachId == _userManager.GetUserId(User));

            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => t.CoachId == _userManager.GetUserId(User)), "Id", "Name", TempData["TeamId"]);

            var positions = from Player.Position p in Enum.GetValues(typeof(Player.Position))
                            select new { Id = (int)p, Name = p.ToString() };
            ViewData["Position"] = new SelectList(positions, "Id", "Name");

            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamId,Name,Surname,DateOfBirth,PlaceOfBirth,Nationality,NaturalPosition,Physical,Role")] Player player)
        {
            var team = await _context.Team.Include(t => t.TeamCompetition).SingleOrDefaultAsync(t => t.Id == player.TeamId);
            if(_userManager.GetUserId(User) != team.CoachId)
            {
                return RedirectToAction("AccessDenied", "Account", "");
            }
            if (ModelState.IsValid)
            {
                _context.Add(player);
                PlayerTeam pt = new PlayerTeam
                {
                    Player = player,
                    PlayerId = player.Id,
                    Team = team,
                    TeamId = player.TeamId
                };
                _context.Add(pt);
                foreach (var c in team.TeamCompetition)
                {
                    _context.Add(new PlayerStats
                    {
                        Team = team,
                        TeamId = player.TeamId,
                        Player = player,
                        PlayerId = player.Id,
                        Year = DateTime.Now,
                        CompetitionId = c.CompetitionId
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Teams", new { id = team.Id });
            }
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player.SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            var teams = _context.Team.Where(t => t.CoachId == _userManager.GetUserId(User));
            ViewData["TeamId"] = new SelectList(teams, "Id", "Name");

            var positions = from Player.Position p in Enum.GetValues(typeof(Player.Position))
                            select new { Id = (int)p, Name = p.ToString() };
            ViewData["Position"] = new SelectList(positions, "Id", "Name");

            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,Name,Surname,DateOfBirth,PlaceOfBirth,Nationality,NaturalPosition,Physical,Role")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Teams", new { id = player.TeamId });
            }
            return View(player);
        }
        
        public IActionResult EditPhysical(int id)
        {
            var player = _context.Player.SingleOrDefault(p => p.Id == id);
            if(player == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    player.Physical = !player.Physical;
                    _context.Update(player);
                    _context.SaveChanges();
                    var result = new
                    {
                        message = $"Igra� {player.Name} {player.Surname} a�uriran.",
                        success = true
                    };
                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = $"Pogre�ka pri a�uriranju: + {exc.InnerException}",
                        success = false
                    };
                    return Json(result);
                }
            }
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Player.SingleOrDefaultAsync(m => m.Id == id);
            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Teams", new { id = player.TeamId });
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.Id == id);
        }
    }
}
