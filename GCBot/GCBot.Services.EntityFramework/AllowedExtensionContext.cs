using GCBot.Services.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Services.EntityFramework
{
    public class AllowedExtensionContext : DbContext
    {
        public AllowedExtensionContext(DbContextOptions<AllowedExtensionContext> options) : base(options) {}
        
        public DbSet<AllowedExtension> AllowedExtensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllowedExtension>().HasIndex(e => e.Value).IsUnique();
        }
    }
}