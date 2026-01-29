using System.ComponentModel.DataAnnotations;

namespace Work_Track.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Foreign key
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Priority { get; set; } = "MED";
        public string Status { get; set; } = "Pending";


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}
