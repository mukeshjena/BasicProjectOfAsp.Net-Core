using Microsoft.EntityFrameworkCore;
using PracticeForRevision.Models;

namespace PracticeForRevision.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<ErrorLogEntry> ErrorLogEntries { get; set; }
    }
}
