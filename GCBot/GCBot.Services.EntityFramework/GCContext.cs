using GCBot.Services.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Services.EntityFramework
{
    public class GCContext : DbContext
    {

        public GCContext(DbContextOptions<GCContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        
        public DbSet<AllowedExtension> AllowedExtensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllowedExtension>().HasIndex(e => e.Value).IsUnique();
        }
    }
}
