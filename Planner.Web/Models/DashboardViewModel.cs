namespace Planner.Web.Models
{
    public class DashboardViewModel
    {
        public List<PlannerEvent> UpcomingEvents { get; set; } = new List<PlannerEvent>();
        public List<TaskItem> PendingTasks { get; set; } = new List<TaskItem>();
        public List<DailyRoutine> TodayRoutines { get; set; } = new List<DailyRoutine>();
    }
}