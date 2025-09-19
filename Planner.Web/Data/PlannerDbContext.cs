using Microsoft.EntityFrameworkCore;
using Planner.Web.Models;

namespace Planner.Web.Data
{
    public class PlannerDbContext : DbContext
    {
        public PlannerDbContext(DbContextOptions<PlannerDbContext> options) : base(options)
        {
        }

        public DbSet<PlannerEvent> Events { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<DailyRoutine> DailyRoutines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Event)
                .WithMany()
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure value conversions for TimeOnly (required for EF Core)
            modelBuilder.Entity<DailyRoutine>()
                .Property(r => r.StartTime)
                .HasConversion(
                    t => t.ToTimeSpan(),
                    t => TimeOnly.FromTimeSpan(t));

            modelBuilder.Entity<DailyRoutine>()
                .Property(r => r.EndTime)
                .HasConversion(
                    t => t.HasValue ? t.Value.ToTimeSpan() : (TimeSpan?)null,
                    t => t.HasValue ? TimeOnly.FromTimeSpan(t.Value) : (TimeOnly?)null);

            // Configure enum properties
            modelBuilder.Entity<PlannerEvent>()
                .Property(e => e.Priority)
                .HasConversion<int>();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Priority)
                .HasConversion<int>();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion<int>();

            modelBuilder.Entity<DailyRoutine>()
                .Property(r => r.Priority)
                .HasConversion<int>();

            modelBuilder.Entity<DailyRoutine>()
                .Property(r => r.DayOfWeek)
                .HasConversion<int>();

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed sample events
            modelBuilder.Entity<PlannerEvent>().HasData(
                new PlannerEvent
                {
                    Id = 1,
                    Title = "Team Meeting",
                    Description = "Weekly team standup meeting",
                    StartDate = DateTime.Today.AddHours(10),
                    EndDate = DateTime.Today.AddHours(11),
                    Location = "Conference Room A",
                    Priority = EventPriority.High,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new PlannerEvent
                {
                    Id = 2,
                    Title = "Project Deadline",
                    Description = "Final submission for Q4 project",
                    StartDate = DateTime.Today.AddDays(7),
                    Priority = EventPriority.Critical,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed sample tasks
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Prepare presentation slides",
                    Description = "Create slides for tomorrow's meeting",
                    DueDate = DateTime.Today.AddDays(1),
                    Priority = TaskPriority.High,
                    Status = Models.TaskStatus.InProgress,
                    Progress = 50,
                    Category = "Work",
                    EventId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Code review",
                    Description = "Review pull requests from team members",
                    DueDate = DateTime.Today,
                    Priority = TaskPriority.Medium,
                    Status = Models.TaskStatus.NotStarted,
                    Category = "Development",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed sample routines
            modelBuilder.Entity<DailyRoutine>().HasData(
                new DailyRoutine
                {
                    Id = 1,
                    Title = "Morning Exercise",
                    Description = "30-minute workout routine",
                    StartTime = new TimeOnly(7, 0),
                    EndTime = new TimeOnly(7, 30),
                    DayOfWeek = DayOfWeek.Monday,
                    Category = "Health",
                    Priority = RoutinePriority.High,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new DailyRoutine
                {
                    Id = 2,
                    Title = "Daily Standup",
                    Description = "Team standup meeting",
                    StartTime = new TimeOnly(9, 30),
                    EndTime = new TimeOnly(9, 45),
                    DayOfWeek = DayOfWeek.Monday,
                    Category = "Work",
                    Priority = RoutinePriority.High,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}