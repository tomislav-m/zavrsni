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

namespace FootballCoachOnline.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Trainings
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Training.Include(t => t.Team);
            if(id != null)
            {
                ViewData["TeamName"] = _context.Team.SingleOrDefault(t => t.Id == id).Name;
                return View(await applicationDbContext.Where(t => t.TeamId == id).ToListAsync());
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Trainings/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => t.CoachId == _userManager.GetUserId(User)), "Id", "Name", TempData["TeamId"]);
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamId,Date,Description")] Training training)
        {
            var team = await _context.Team.SingleOrDefaultAsync(t => t.Id == training.TeamId);
            if(_userManager.GetUserId(User) != team.CoachId)
            {
                return RedirectToAction("AccessDenied", "Account", "");
            }
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Name", training.TeamId);
            return View(training);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training.SingleOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Name", training.TeamId);
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,Date,Description")] Training training)
        {
            if (id != training.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.Id))
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
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Age", training.TeamId);
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Training.SingleOrDefaultAsync(m => m.Id == id);
            _context.Training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TrainingExists(int id)
        {
            return _context.Training.Any(e => e.Id == id);
        }
    }
}
