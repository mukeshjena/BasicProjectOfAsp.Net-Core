namespace PracticeForRevision.Models
{
    public class ErrorLogEntry
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
