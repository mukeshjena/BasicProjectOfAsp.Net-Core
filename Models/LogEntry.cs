namespace PracticeForRevision.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
