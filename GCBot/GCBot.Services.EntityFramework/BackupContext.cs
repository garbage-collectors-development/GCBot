using GCBot.Services.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Services.EntityFramework
{
    public class GCContext : DbContext
    {
        private string _connectionString;

        public GCContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySql(_connectionString);
        }
    }
}
