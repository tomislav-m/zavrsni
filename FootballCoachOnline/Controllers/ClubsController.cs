using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Models;
using Microsoft.AspNetCore.Authorization;
using FootballCoachOnline.Data;

namespace FootballCoachOnline.Controllers
{
    public class ClubsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClubsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Clubs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Club.OrderBy(c => c.Name).ToListAsync());
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .SingleOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [Authorize]
        // GET: Clubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Nickname,YearFounded")] Club club)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(club);
                    await _context.SaveChangesAsync();

                    TempData[Constants.Message] = $"Klub {club.Name} uspje�no dodan.";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction("Index");
                }
                catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.ToString());
                    TempData[Constants.Message] = "Pogre�ka u dodavanju kluba";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club.SingleOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Nickname,YearFounded")] Club club)
        {
            if (id != club.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.Id))
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
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .SingleOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _context.Club.SingleOrDefaultAsync(m => m.Id == id);
            try
            {
                _context.Club.Remove(club);
                await _context.SaveChangesAsync();

                TempData[Constants.Message] = $"Klub {club.Name} uspje�no obrisan.";
                TempData[Constants.ErrorOccurred] = false;
            }
            catch (DbUpdateException exc)
            {
                ModelState.AddModelError(string.Empty, exc.ToString());
                TempData[Constants.Message] = $"Klub {club.Name} ne mo�e biti obrisan jer ima timove!";
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction("Index", "Clubs", "");
        }

        private bool ClubExists(int id)
        {
            return _context.Club.Any(e => e.Id == id);
        }
    }
}
