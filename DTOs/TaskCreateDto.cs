using System.ComponentModel.DataAnnotations;

namespace Work_Track.DTOs
{
    public class TaskCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "MED";
        public string? Description { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }

    }
}
