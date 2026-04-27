using Microsoft.EntityFrameworkCore;
using AprobadorIdeas.Models;

namespace AprobadorIdeas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Idea> Ideas { get; set; } = null!;
    }
}
