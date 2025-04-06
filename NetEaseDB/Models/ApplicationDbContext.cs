using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NetEaseDB.Models
{
    // Represents the application's database context used by Entity Framework Core
    public class ApplicationDbContext : DbContext
    {
        // Constructor: accepts configuration options and passes them to the base DbContext class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // DbSet representing the tables in the database
        public DbSet<Venue> Venues { get; set; }
        public DbSet<EventInfo> EventInfo { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
