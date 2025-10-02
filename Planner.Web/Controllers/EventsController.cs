using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Web.Data;
using Planner.Web.Models;

namespace Planner.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly PlannerDbContext _context;

        public EventsController(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .OrderBy(e => e.StartDate)
                .ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannerEvent = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plannerEvent == null)
            {
                return NotFound();
            }

            return View(plannerEvent);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate,EndDate,Location,Priority")] PlannerEvent plannerEvent)
        {
            if (ModelState.IsValid)
            {
                plannerEvent.CreatedAt = DateTime.UtcNow;
                plannerEvent.UpdatedAt = DateTime.UtcNow;
                _context.Add(plannerEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plannerEvent);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannerEvent = await _context.Events.FindAsync(id);
            if (plannerEvent == null)
            {
                return NotFound();
            }
            return View(plannerEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,Location,Priority,IsCompleted,CreatedAt")] PlannerEvent plannerEvent)
        {
            if (id != plannerEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    plannerEvent.UpdatedAt = DateTime.UtcNow;
                    _context.Update(plannerEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlannerEventExists(plannerEvent.Id))
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
            return View(plannerEvent);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannerEvent = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plannerEvent == null)
            {
                return NotFound();
            }

            return View(plannerEvent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plannerEvent = await _context.Events.FindAsync(id);
            if (plannerEvent != null)
            {
                _context.Events.Remove(plannerEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlannerEventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}