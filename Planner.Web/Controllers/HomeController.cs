using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Web.Data;
using Planner.Web.Models;

namespace Planner.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PlannerDbContext _context;

    public HomeController(ILogger<HomeController> logger, PlannerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            UpcomingEvents = await _context.Events
                .Where(e => e.StartDate >= DateTime.Today && !e.IsCompleted)
                .OrderBy(e => e.StartDate)
                .Take(5)
                .ToListAsync(),
            PendingTasks = await _context.Tasks
                .Where(t => t.Status != Models.TaskStatus.Completed && t.Status != Models.TaskStatus.Cancelled)
                .OrderBy(t => t.DueDate)
                .Take(10)
                .ToListAsync(),
            TodayRoutines = await _context.DailyRoutines
                .Where(r => r.IsActive && r.DayOfWeek == DateTime.Today.DayOfWeek)
                .OrderBy(r => r.StartTime)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
