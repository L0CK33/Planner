using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Web.Data;
using Planner.Web.Models;

namespace Planner.Web.Controllers
{
    public class RoutinesController : Controller
    {
        private readonly PlannerDbContext _context;

        public RoutinesController(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var routines = await _context.DailyRoutines
                .Where(r => r.IsActive)
                .OrderBy(r => r.DayOfWeek)
                .ThenBy(r => r.StartTime)
                .ToListAsync();
            return View(routines);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routine = await _context.DailyRoutines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (routine == null)
            {
                return NotFound();
            }

            return View(routine);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartTime,EndTime,DayOfWeek,IsActive,Category,Priority")] DailyRoutine routine)
        {
            if (ModelState.IsValid)
            {
                routine.CreatedAt = DateTime.UtcNow;
                routine.UpdatedAt = DateTime.UtcNow;
                _context.Add(routine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(routine);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routine = await _context.DailyRoutines.FindAsync(id);
            if (routine == null)
            {
                return NotFound();
            }
            return View(routine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartTime,EndTime,DayOfWeek,IsActive,Category,Priority,CreatedAt")] DailyRoutine routine)
        {
            if (id != routine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    routine.UpdatedAt = DateTime.UtcNow;
                    _context.Update(routine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoutineExists(routine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(routine);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routine = await _context.DailyRoutines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (routine == null)
            {
                return NotFound();
            }

            return View(routine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routine = await _context.DailyRoutines.FindAsync(id);
            if (routine != null)
            {
                _context.DailyRoutines.Remove(routine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoutineExists(int id)
        {
            return _context.DailyRoutines.Any(e => e.Id == id);
        }
    }
}