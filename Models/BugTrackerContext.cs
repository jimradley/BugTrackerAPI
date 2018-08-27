using Microsoft.EntityFrameworkCore;

namespace BugTrackerApi.Models
{
    public class BugTrackerContext : DbContext
    {
        public BugTrackerContext(DbContextOptions<BugTrackerContext> contextOptions) : base(contextOptions)
        {            
        }

        public DbSet<Bug> Bugs {get; set;}

        public DbSet<User> Users {get; set;}
    }
}