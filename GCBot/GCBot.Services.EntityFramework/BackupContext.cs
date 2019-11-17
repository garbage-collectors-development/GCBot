using GCBot.Services.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Services.EntityFramework
{
    public class BackupContext : DbContext
    {
        private string _connectionString;

        public BackupContext(string connectionString)
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
