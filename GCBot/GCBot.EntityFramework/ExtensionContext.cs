using GCBot.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCBot.EntityFramework
{
    public class ExtensionContext : DbContext
    {
        public ExtensionContext(DbContextOptions<ExtensionContext> options) : base(options) {}
        
        public DbSet<Extension> AllowedExtensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Extension>().HasIndex(e => e.Value).IsUnique();
        }
    }
}