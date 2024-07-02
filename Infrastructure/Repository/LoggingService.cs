using PracticeForRevision.DAL;
using PracticeForRevision.Infrastructure.Interface;
using PracticeForRevision.Models;

namespace PracticeForRevision.Infrastructure.Repository
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public LoggingService(ILogger<LoggingService> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task LogActionAsync(string actionName, string description)
        {
            var logMessage = $"{DateTime.Now} - {actionName}: {description}";
            LogToFile(logMessage);
            await LogToDatabaseAsync(actionName, description);
        }

        public async Task LogErrorAsync(Exception ex, string actionName = null)
        {
            var logMessage = $"{DateTime.Now} - ERROR{(actionName != null ? $" in {actionName}" : "")}: {ex.Message}";
            LogToFile(logMessage);

            // Log to database
            var errorEntry = new ErrorLogEntry
            {
                ActionName = actionName,
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                Timestamp = DateTime.Now
            };

            _dbContext.ErrorLogEntries.Add(errorEntry);
            await _dbContext.SaveChangesAsync();
        }

        private void LogToFile(string logMessage)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "applog.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.AppendAllText(filePath, logMessage + Environment.NewLine);
        }

        private async Task LogToDatabaseAsync(string actionName, string description)
        {
            var logEntry = new LogEntry
            {
                ActionName = actionName,
                Description = description,
                Timestamp = DateTime.Now
            };

            _dbContext.LogEntries.Add(logEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
