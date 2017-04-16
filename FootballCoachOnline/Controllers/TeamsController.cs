using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Models;
using FootballCoachOnline.ViewModels;
using FootballCoachOnline.Data;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace FootballCoachOnline.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public TeamsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Teams
        public async Task<IActionResult> Index(int id=0)
        {
            if (id != 0)
            {
                return View(await _context.Team.Where(t => t.ClubId == id).Include(t => t.Club).OrderBy(t => t.Name).ToListAsync());
            }
            return View(await _context.Team.Include(t => t.Club).OrderBy(t => t.Name).ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            if (!(await authorize(team)))
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }
            
            var players = _context.PlayerTeam.Where(t => t.TeamId == id).Select(p => p.Player);
            ViewData["Team"] = team.Name;

            return View(players);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Club.OrderBy(t => t.Name), "Id", "Name");
            ViewData["CoachId"] = new SelectList(userManager.Users, "Id", "UserName");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoachId,ClubId,Name,Age")] Team team)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(team);
                    await _context.SaveChangesAsync();

                    TempData[Constants.Message] = $"Ekipa {team.Name} uspješno dodana.";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction("Index");
                }
                catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.ToString());
                    TempData[Constants.Message] = "Pogreška u dodavanju tima";
                    TempData[Constants.ErrorOccurred] = false;
                }
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Name", team.ClubId);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            if (!(await authorize(team)))
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }

            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Name", team.ClubId);
            ViewData["CoachId"] = new SelectList(userManager.Users, "Id", "UserName");
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoachId,ClubId,Name,Age")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
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
            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Name", team.ClubId);
            ViewData["CoachId"] = new SelectList(userManager.Users, "Id", "UserName");
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .Include(t => t.Club)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            if (!(await authorize(team)))
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            int clubId = team.ClubId;
            var tc = _context.TeamCompetition.Where(t => t.TeamId == id);
            _context.TeamCompetition.RemoveRange(tc);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = clubId });
        }

        // GET: Teams/Join/5
        public async Task<IActionResult> Join(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var team = await _context.Team
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            if (await authorize(team))
            {
                var comp = from tc in _context.TeamCompetition
                           join c in _context.Competition on tc.CompetitionId equals c.Id
                           where tc.TeamId == id
                           select c;

                var competitions = _context.Competition.Except(comp);


                var competitionVM = new CompetitionViewModel
                {
                    CompetitionSL = new SelectList(competitions, "Id", "Name"),
                    Team = team
                };

                return View(competitionVM);
            }

            return RedirectToAction("AccessDenied", "Account", new { area = "" });
        }

        // POST: Teams/Join/5
        [HttpPost]
        public async Task<IActionResult> Join(int id, int competitionId)
        {
            if(id == 0 || competitionId == 0)
            {
                return NotFound();
            }
            try
            {
                var team = await _context.Team.SingleOrDefaultAsync(t => t.Id == id);
                var competition = await _context.Competition.SingleOrDefaultAsync(c => c.Id == competitionId);

                _context.TeamCompetition.Add(new TeamCompetition
                {
                    TeamId = id,
                    Team = team,
                    CompetitionId = competitionId,
                    Competition = competition
                });
                _context.TeamStats.Add(new TeamStats
                {
                    CompetitionId = competitionId,
                    TeamId = id,
                    Year = DateTime.Now.Year
                });
                await _context.SaveChangesAsync();

                TempData[Constants.Message] = $"Tim {team.Name} se uspješno pridružio natjecanju {competition.Name}";
                TempData[Constants.ErrorOccurred] = false;
            }
            catch(Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.ToString());
                TempData[Constants.Message] = "Pogreška u pridruživanju natjecanju.";
                TempData[Constants.ErrorOccurred] = false;
            }

            return RedirectToAction("TeamCompetition", new { id = id });
        }

        // GET: Teams/TeamCompetition/5
        public async Task<IActionResult> TeamCompetition(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var team = await _context.Team.SingleOrDefaultAsync(t => t.Id == id);
            if(team == null)
            {
                return NotFound();
            }

            if (await authorize(team))
            {

                var competitions = from tc in _context.TeamCompetition
                                   join c in _context.Competition on tc.CompetitionId equals c.Id
                                   where tc.TeamId == id
                                   select c;

                var VM = new TeamCompetitionViewModel
                {
                    Competitions = competitions,
                    Team = team
                };

                return View(VM);
            }
            return RedirectToAction("AccessDenied", "Account", new { area = "" });
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Leave(int teamId, int competitionId)
        {
            if(teamId != 0)
            {
                try
                {
                    var teamcompetiton = await _context.TeamCompetition
                                     .SingleOrDefaultAsync(tc => tc.TeamId == teamId && tc.CompetitionId == competitionId);

                    if (teamcompetiton != null)
                    {
                        _context.TeamCompetition.Remove(teamcompetiton);
                        await _context.SaveChangesAsync();

                        TempData[Constants.Message] = $"Tim je uspješno napustio natjecanje";
                        TempData[Constants.ErrorOccurred] = false;
                    }
                }
                catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.ToString());
                    TempData[Constants.Message] = "Pogreška u napuštanju natjecanja.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                return RedirectToAction("TeamCompetition", new { id = teamId });
            }
            return RedirectToAction("Index");
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }

        public async Task<bool> authorize(Team team)
        {
            return (team.CoachId != null &&
                    (team.CoachId == userManager.GetUserId(User) || 
                    await userManager.IsInRoleAsync(await userManager.GetUserAsync(User), "Administrator")));
        }
    }
}
