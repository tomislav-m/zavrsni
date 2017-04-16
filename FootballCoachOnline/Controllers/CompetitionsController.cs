using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Authorization;
using FootballCoachOnline.Data;
using FootballCoachOnline.ViewModels;

namespace FootballCoachOnline.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Competition.ToListAsync());
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competition
                .SingleOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            ViewData["CompetitionName"] = competition.Name;

            var stats = _context.TeamStats.Where(ts => ts.CompetitionId == id).ToList();

            List<CompetitionTeamStatsViewModel> cts = new List<CompetitionTeamStatsViewModel>();

            foreach(var item in stats)
            {
                cts.Add(new CompetitionTeamStatsViewModel {
                    Draws = item.Draws,
                    GamesPlayed = item.GamesPlayed,
                    GoalsConceded = item.GoalsConceded,
                    GoalsScored = item.GoalsScored,
                    Losses = item.Losses,
                    Name = await _context.Team.Where(t => t.Id == item.TeamId).Select(t => t.Name).SingleOrDefaultAsync(),
                    Points = item.Draws + item.Wins * 3,
                    Wins = item.Wins
                });
            }

            return View(cts.OrderBy(t => t.Points).ThenBy(t => t.GoalsScored - t.GoalsConceded).ThenBy(t => t.Name).ToList());
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competition.SingleOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Competition competition)
        {
            if (id != competition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // GET: Competitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competition
                .SingleOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competition = await _context.Competition.SingleOrDefaultAsync(m => m.Id == id);
            _context.Competition.Remove(competition);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CompetitionExists(int id)
        {
            return _context.Competition.Any(e => e.Id == id);
        }
    }
}
