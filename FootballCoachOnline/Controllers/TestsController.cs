using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Data;
using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Identity;

namespace FootballCoachOnline.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;    
        }

        // GET: Tests
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Test.Include(t => t.Player);
            if (id == null)
            {
                return View(await applicationDbContext.ToListAsync());
            }
            var players = _context.PlayerTeam.Where(t => t.TeamId == id).Select(p => p.PlayerId);
            var tests = _context.Test.Where(t => players.Contains(t.PlayerId)).Include(p => p.Player);
            ViewData["teamName"] = " - " + _context.Team.SingleOrDefault(t => t.Id == id).Name;
            
            return View(tests);
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Player)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: Tests/Create
        public IActionResult Create(int? playerId)
        {
            var players = _context.Team.
                Where(t => t.CoachId == _userManager.GetUserId(User))
                .SelectMany(t => t.PlayerTeam)
                .Select(t => t.Player);
            
            ViewData["Players"] = new SelectList(players, "Id", "FullName", playerId.GetValueOrDefault());
            ViewData["PlayerId"] = playerId;
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayerId,Name,Description,Date")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Players", new {id = test.PlayerId});
            }
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Name", test.PlayerId);
            return View(test);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test.Include(p => p.Player).SingleOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayerId,Name,Description,Date")] Test test)
        {
            if (id != test.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Players", new {id = test.PlayerId});
            }
            return View(test);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Player)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Test.SingleOrDefaultAsync(m => m.Id == id);
            _context.Test.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.Id == id);
        }
    }
}
