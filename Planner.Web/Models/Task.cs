using System.ComponentModel.DataAnnotations;

namespace Planner.Web.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

        [Range(0, 100)]
        public int Progress { get; set; } = 0;

        [StringLength(100)]
        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for related event
        public int? EventId { get; set; }
        public PlannerEvent? Event { get; set; }
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum TaskStatus
    {
        NotStarted = 1,
        InProgress = 2,
        Completed = 3,
        OnHold = 4,
        Cancelled = 5
    }
}