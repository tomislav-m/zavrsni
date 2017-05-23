using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Data;
using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FootballCoachOnline.ViewModels;

namespace FootballCoachOnline.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MatchesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Matches
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Match
                                               .Include(m => m.Competition)
                                               .Include(m => m.MatchScore)
                                               .Include(m => m.Team1)
                                               .Include(m => m.Team2)
                                               .OrderBy(m => m.Date);
            if(id != null)
            {
                var team = await _context.Team.SingleOrDefaultAsync(t => t.Id == id);
                if(team == null)
                {
                    return NotFound();
                }
                ViewData["Team"] = " - " + team.Name;
                return View(await applicationDbContext.Where(m => m.Team1Id == id || m.Team2Id == id).ToListAsync());
            }
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
                .Include(m => m.MatchStats)
                .ThenInclude(m => m.Player)
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
            ViewData["Team1Id"] = new SelectList(_context.Team.OrderBy(t => t.Name), "Id", "Name");
            ViewData["Team2Id"] = new SelectList(_context.Team.OrderBy(t => t.Name), "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompetitionId,Team1Id,Team2Id,Date,Played,Matchday")] Match match, int? home, int? away)
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
                if (match.Played)
                {
                    UpdateStats(match, null);
                }
                await _context.SaveChangesAsync();

                //UpdateStats(match);

                return RedirectToAction("Details", "Competitions", new { id = match.CompetitionId});
            }
            ViewData["CompetitionId"] = match.CompetitionId;
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

            var match = await _context.Match
                              .Include(m => m.MatchScore)
                              .SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompetitionId,MatchScoreId,Team1Id,Team2Id,Date,Played,Matchday")] Match match, int? home, int?away)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MatchScore mc = _context.MatchScore.Where(m => m.Match == match).SingleOrDefault();
                    if (match.Played && home != null && away != null)
                    {
                        mc.Score1 = home.GetValueOrDefault();
                        mc.Score2 = away.GetValueOrDefault();
                    }

                    var oldMatch = _context.Match.Include(m => m.MatchScore).AsNoTracking().SingleOrDefault(m => m.Id == match.Id);

                    UpdateStats(match, oldMatch);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var match = await _context.Match.SingleOrDefaultAsync(m => m.Id == id);
            try
            {
                _context.Match.Remove(match);
                await _context.SaveChangesAsync();

                var result = new
                {
                    message = "Utakmica uspješno obrisana",
                    success = true
                };
                return Json(result);
            }
            catch (Exception exc)
            {
                var result = new
                {
                    message = $"Pogreška pri brisanju lokacije: {exc.InnerException}",
                    success = false
                };
                return Json(result);
            }
        }

        private bool MatchExists(int id)
        {
            return _context.Match.Any(e => e.Id == id);
        }

        public void UpdateStatsAll()
        {
            var stats = _context.TeamStats.ToList();

            foreach(var s in stats)
            {
                s.GamesPlayed = 0;
                s.GoalsScored = 0;
                s.GoalsConceded = 0;
                s.Draws = 0;
                s.Losses = 0;
                s.Wins = 0;
                _context.Update(s);
                _context.SaveChanges();
            }

            var matches = _context.Match.Include(m => m.MatchScore).ToList();

            foreach (var match in matches)
            {
                if (match.Played)
                {
                    UpdateStats(match, null);
                    _context.SaveChanges();
                }
            }
        }

        public void UpdateStats(Match match, Match oldMatch)
        {
            var team1 = _context.TeamStats.Where(s => s.CompetitionId == match.CompetitionId)
                                .FirstOrDefault(t => t.TeamId == match.Team1Id);
            var team2 = _context.TeamStats.Where(s => s.CompetitionId == match.CompetitionId)
                        .FirstOrDefault(t => t.TeamId == match.Team2Id);

            var score1 = _context.MatchScore.SingleOrDefault(m => m.Match == match).Score1;
            var score2 = _context.MatchScore.SingleOrDefault(m => m.Match == match).Score2;

            if (oldMatch != null && oldMatch.Played)
            {
                team1.GoalsScored -= oldMatch.MatchScore.Score1;
                team2.GoalsScored -= oldMatch.MatchScore.Score2;
                team1.GoalsConceded -= oldMatch.MatchScore.Score2;
                team2.GoalsConceded -= oldMatch.MatchScore.Score1;
                team1.GamesPlayed--;
                team2.GamesPlayed--;

                if (oldMatch.MatchScore.Score1 > oldMatch.MatchScore.Score2)
                {
                    team1.Wins--;
                    team2.Losses--;
                }
                else if (oldMatch.MatchScore.Score1 < oldMatch.MatchScore.Score2)
                {
                    team2.Wins--;
                    team1.Losses--;
                }
                else
                {
                    team1.Draws--;
                    team2.Draws--;
                }
            }

            if (match.Played)
            {
                team1.GoalsScored += score1;
                team2.GoalsScored += score2;
                team1.GoalsConceded += score2;
                team2.GoalsConceded += score1;
                team1.GamesPlayed++;
                team2.GamesPlayed++;

                if (score1 > score2)
                {
                    team1.Wins++;
                    team2.Losses++;
                }
                else if (score2 > score1)
                {
                    team2.Wins++;
                    team1.Losses++;
                }
                else
                {
                    team1.Draws++;
                    team2.Draws++;
                }
            }

            _context.Update(team1);
            _context.Update(team2);
        }

        public IActionResult GenerateMatches(int? id)
        {
            var competition = _context.Competition.SingleOrDefault(c => c.Id == id);

            if (competition == null)
            {
                return NotFound();
            }

            var teams = from t in _context.Team
                        join tc in _context.TeamCompetition
                        on t.Id equals tc.TeamId
                        where tc.CompetitionId == id
                        select t;

            var teams2 = new List<Team>();
            teams2.AddRange(teams);

            bool flag = false;

            foreach(var t in teams.ToList())
            {
                flag = false;
                foreach(var t2 in teams2)
                {
                    foreach(var m in _context.Match.Where(x => x.Team1Id == t.Id && x.Team2Id == t2.Id).ToList())
                    {
                        flag = true;
                        break;
                    }
                    if (flag || t.Id == t2.Id) continue;

                    MatchScore matchScore = new MatchScore();

                    Match match = new Match
                    {
                        CompetitionId = id,
                        Team1Id = t.Id,
                        Team2Id = t2.Id,
                        MatchScore = matchScore
                    };
                    matchScore.Match = match;

                    _context.Add(match);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Details", "Competitions", new {id});
        }

        [Authorize]
        public async Task<IActionResult> AddPlayers(int id)
        {
            var match = await _context.Match
                        .Include(m => m.Team1)
                        .Include(m => m.Team2)
                        .SingleOrDefaultAsync(m => m.Id == id);
            if(match == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            int teamId = 0;
            if(match.Team1.CoachId == userId)
            {
                teamId = match.Team1Id;
            }
            else if(match.Team2.CoachId == userId)
            {
                teamId = match.Team2Id;
            }
            if (teamId == 0)
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }
            var players = _context.PlayerTeam.Where(t => t.TeamId == teamId)
                          .Select(p => p.Player)
                          .Include(p => p.MatchStats)
                          .OrderBy(p => p.NaturalPosition)
                          .ToList();

            List<object> names = new List<object>();
            foreach(var player in players)
            {
                names.Add(new
                {
                    Id = player.Id,
                    Name = player.Name + " " + player.Surname
                });
            }
            var team = await _context.Team.SingleOrDefaultAsync(t => t.Id == teamId);
            ViewData["teamId"] = teamId;

            var vm = new PlayersMatchViewModel
            {
                PlayersSL = new SelectList(names, "Id", "Name"),
                Match = match,
                Players = players
            };
            if(match.MatchStats != null)
            {
                vm.MatchStats = match.MatchStats.ToList();
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPlayers(int matchId, int teamId, bool app, bool sub, int goals, int concededGoals, bool yellowCard, bool redCard, int playerId)
        {
            var player = _context.Player.SingleOrDefault(p => p.Id == playerId);
            try
            {
                var matchStatsOld = _context.MatchStats.SingleOrDefault(m => m.PlayerId == playerId && m.MatchId == matchId);
                if (matchStatsOld == null)
                {
                    var matchStats = new MatchStats
                    {
                        App = app,
                        Sub = sub,
                        Goals = goals,
                        GoalsConceded = concededGoals,
                        RedCard = redCard,
                        YellowCard = yellowCard,
                        TeamId = teamId,
                        MatchId = matchId,
                        PlayerId = playerId,
                    };

                    _context.MatchStats.Add(matchStats);
                }
                else
                {
                    matchStatsOld.App = app;
                    matchStatsOld.Sub = sub;
                    matchStatsOld.Goals = goals;
                    matchStatsOld.GoalsConceded = concededGoals;
                    matchStatsOld.RedCard = redCard;
                    matchStatsOld.YellowCard = yellowCard;

                    _context.Update(matchStatsOld);
                }

                await _context.SaveChangesAsync();

                var result = new
                {
                    message = $"Igrač {player.FullName} uspješno dodan.",
                    success = true
                };
                return Json(result);
            }
            catch(Exception exc)
            {
                var result = new
                {
                    message = $"Neuspješno dodavanje igrača {player.FullName}!",
                    success = false
                };
                return Json(result);
            }
        }
    }
}
