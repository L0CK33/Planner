using System.ComponentModel.DataAnnotations;

namespace Planner.Web.Models
{
    public class DailyRoutine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? Category { get; set; }

        public RoutinePriority Priority { get; set; } = RoutinePriority.Medium;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum RoutinePriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
}