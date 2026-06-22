using Microsoft.EntityFrameworkCore;
using Morgan.SalesforceSyncPOC.Core.DataModels;


namespace Morgan.SalesforceSyncPOC.Infrastrcture.Data
{
    /// <summary>
    /// Application database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes the database context.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}
