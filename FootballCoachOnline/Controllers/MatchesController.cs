using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Data;
using FootballCoachOnline.Models;

namespace FootballCoachOnline.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatchesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Match.Include(m => m.Competition).Include(m => m.MatchScore).Include(m => m.Team1).Include(m => m.Team2);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Match
                .Include(m => m.Competition)
                .Include(m => m.MatchScore)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public IActionResult Create(int id)
        {
            ViewData["CompetitionId"] = id;
            ViewData["Team1Id"] = new SelectList(_context.Team, "Id", "Name");
            ViewData["Team2Id"] = new SelectList(_context.Team, "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompetitionId,Team1Id,Team2Id,Date")] Match match, int? home, int? away)
        {
            if (ModelState.IsValid)
            {
                var matchScore = new MatchScore();

                if(home != null && away != null)
                {
                    matchScore.Score1 = home.GetValueOrDefault();
                    matchScore.Score2 = away.GetValueOrDefault();
                }
                matchScore.Match = match;
                match.MatchScore = matchScore;

                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", match.CompetitionId);
            ViewData["MatchScoreId"] = new SelectList(_context.MatchScore, "Id", "Id", match.MatchScoreId);
            ViewData["Team1Id"] = new SelectList(_context.Team, "Id", "Age", match.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Team, "Id", "Age", match.Team2Id);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Match.SingleOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["Team1Id"] = new SelectList(_context.Team, "Id", "Name", match.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Team, "Id", "Name", match.Team2Id);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompetitionId,MatchScoreId,Team1Id,Team2Id,Date")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
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
            ViewData["Team1Id"] = new SelectList(_context.Team, "Id", "Name", match.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Team, "Id", "Name", match.Team2Id);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Match
                .Include(m => m.Competition)
                .Include(m => m.MatchScore)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Match.SingleOrDefaultAsync(m => m.Id == id);
            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MatchExists(int id)
        {
            return _context.Match.Any(e => e.Id == id);
        }
    }
}
