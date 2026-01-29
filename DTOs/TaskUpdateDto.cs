namespace Work_Track.DTOs
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "MED";
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
