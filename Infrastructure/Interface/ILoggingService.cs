namespace PracticeForRevision.Infrastructure.Interface
{
    public interface ILoggingService
    {
        Task LogActionAsync(string actionName, string description);
        Task LogErrorAsync(Exception ex, string actionName = null);
    }
}
