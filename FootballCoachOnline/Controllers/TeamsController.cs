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

namespace FootballCoachOnline.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TeamsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

            var team = await _context.Team
                             .Include(m => m.MatchTeam1)
                             .ThenInclude(m => m.MatchScore)
                             .Include(m => m.MatchTeam1)
                             .ThenInclude(m => m.Team2)
                             .Include(m => m.MatchTeam1)
                             .ThenInclude(m => m.Competition)
                             .Include(m => m.MatchTeam2)
                             .ThenInclude(m => m.MatchScore)
                             .Include(m => m.MatchTeam2)
                             .ThenInclude(m => m.Team1)
                             .Include(m => m.MatchTeam2)
                             .ThenInclude(m => m.Competition)
                             .SingleOrDefaultAsync(m => m.Id == id);
            
            if (team == null)
            {
                return NotFound();
            }

            var allStats = _context.PlayerStats
                .Where(s => s.TeamId == id)
                .Include(p => p.Player)
                .OrderBy(s => s.Player.NaturalPosition)
                .ToList();
            var playerStats = new Dictionary<int, PlayerStats>();
            foreach (var s in allStats)
            {
                if (playerStats.ContainsKey(s.PlayerId))
                {
                    playerStats[s.PlayerId].Apps += s.Apps;
                    playerStats[s.PlayerId].Subs += s.Subs;
                    playerStats[s.PlayerId].Goals += s.Goals;
                    playerStats[s.PlayerId].GoalsConceded += s.GoalsConceded;
                    playerStats[s.PlayerId].RedCards += s.RedCards;
                    playerStats[s.PlayerId].YellowCards += s.YellowCards;
                }
                else
                {
                    var stats = new PlayerStats
                    {
                        Player = s.Player,
                        Apps = s.Apps,
                        Goals = s.Goals,
                        GoalsConceded = s.GoalsConceded,
                        RedCards = s.RedCards,
                        Subs = s.Subs,
                        YellowCards = s.YellowCards
                    };
                    playerStats.Add(s.PlayerId, stats);
                }
            }

            if (!(await Authorize(team)))
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }

            var matches = team.MatchTeam1.ToList();
            matches.AddRange(team.MatchTeam2);
            var matchesP = matches.Where(m => m.Date.Date <= DateTime.Now.Date).OrderByDescending(m => m.Date).Take(5).ToList();
            var matchesF = matches.Where(m => m.Date.Date > DateTime.Now.Date).OrderByDescending(m => m.Date).Take(10-matchesP.Count).ToList();
            matches.Clear();
            matches.AddRange(matchesF);
            matches.AddRange(matchesP);

            var trainings = _context.Training.Where(t => t.TeamId == id)
                            .OrderByDescending(t => t.Date)
                            .Take(5)
                            .ToList();

            var vm = new PlayersMatchesViewModel
            {
                Matches = matches.OrderBy(m => m.Date).ToList(),
                PlayerStats = playerStats,
                Team = team,
                Trainings = trainings
            };

            ViewData["Team"] = team.Name;
            ViewData["TeamId"] = team.Id;

            return View(vm);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Club.OrderBy(t => t.Name), "Id", "Name");
            ViewData["CoachId"] = new SelectList(_userManager.Users, "Id", "UserName");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoachId,ClubId,Name,Age,ShortName")] Team team)
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

            if (!(await Authorize(team)))
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }

            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Name", team.ClubId);
            ViewData["CoachId"] = new SelectList(_userManager.Users, "Id", "UserName");
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoachId,ClubId,Name,Age,ShortName")] Team team)
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
            ViewData["CoachId"] = new SelectList(_userManager.Users, "Id", "UserName");
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

            if (!(await Authorize(team)))
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

            if (await Authorize(team))
            {
                var comp = from tc in _context.TeamCompetition
                           join c in _context.Competition on tc.CompetitionId equals c.Id
                           where tc.TeamId == id
                           select c;

                var competitions = _context.Competition.Except(comp);


                var competitionvm = new CompetitionViewModel
                {
                    CompetitionSL = new SelectList(competitions, "Id", "Name"),
                    Team = team
                };

                return View(competitionvm);
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
                var team = await _context.Team.Include(t => t.PlayerTeam).SingleOrDefaultAsync(t => t.Id == id);
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
                foreach(var player in team.PlayerTeam)
                {
                    var playerStats = _context.PlayerStats
                        .SingleOrDefault(p => p.CompetitionId == competitionId && p.TeamId == team.Id && p.PlayerId == player.PlayerId && p.Year.Year == DateTime.Now.Year);
                    if (playerStats != null)
                    {
                        continue;
                    }
                    _context.Add(new PlayerStats
                    {
                        TeamId = player.TeamId,
                        PlayerId = player.PlayerId,
                        Year = DateTime.Now,
                        CompetitionId = competition.Id
                    });
                }
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

            if (await Authorize(team))
            {

                var competitions = from tc in _context.TeamCompetition
                                   join c in _context.Competition on tc.CompetitionId equals c.Id
                                   where tc.TeamId == id
                                   select c;

                var vm = new TeamCompetitionViewModel
                {
                    Competitions = competitions,
                    Team = team
                };

                return View(vm);
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

                        TempData[Constants.Message] = "Tim je uspješno napustio natjecanje";
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

        public async Task<bool> Authorize(Team team)
        {
            if (!_signInManager.IsSignedIn(User)){
                return false;
            }

            if(await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Administrator"))
            {
                return true;
            }

            return (team.CoachId != null && (team.CoachId == _userManager.GetUserId(User)));
        }
        
        public IActionResult GetTable(int id)
        {
            var team = _context.Team.SingleOrDefault(c => c.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            
            var allStats = _context.PlayerStats
                .Where(s => s.TeamId == id)
                .Include(p => p.Player)
                .OrderBy(s => s.Player.NaturalPosition)
                .ToList();
            var playerStats = new Dictionary<int, PlayerStats>();
            foreach (var s in allStats)
            {
                if (playerStats.ContainsKey(s.PlayerId))
                {
                    playerStats[s.PlayerId].Apps += s.Apps;
                    playerStats[s.PlayerId].Subs += s.Subs;
                    playerStats[s.PlayerId].Goals += s.Goals;
                    playerStats[s.PlayerId].GoalsConceded += s.GoalsConceded;
                    playerStats[s.PlayerId].RedCards += s.RedCards;
                    playerStats[s.PlayerId].YellowCards += s.YellowCards;
                }
                else
                {
                    var stats = new PlayerStats
                    {
                        Player = s.Player,
                        Apps = s.Apps,
                        Goals = s.Goals,
                        GoalsConceded = s.GoalsConceded,
                        RedCards = s.RedCards,
                        Subs = s.Subs,
                        YellowCards = s.YellowCards
                    };
                    playerStats.Add(s.PlayerId, stats);
                }
            }
            
            List<object> list = new List<object>();

            int i = 1;
            foreach(var item in playerStats.Values)
            {
                var result = new
                {
                    ime = item.Player.FullName,
                    utakmice = item.Apps,
                    zamjene = item.Subs,
                    golovi = item.Goals,
                    primljeni = item.GoalsConceded,
                    žuti = item.YellowCards,
                    crveni = item.RedCards
                };
                list.Add(result);
            }

            return Json(list);
        }
    }
}
