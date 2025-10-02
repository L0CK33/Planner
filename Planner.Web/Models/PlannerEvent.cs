using System.ComponentModel.DataAnnotations;

namespace Planner.Web.Models
{
    public class PlannerEvent
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        public EventPriority Priority { get; set; } = EventPriority.Medium;

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum EventPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
}