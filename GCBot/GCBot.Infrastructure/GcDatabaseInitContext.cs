using System.Diagnostics.Contracts;
using Discord.Addons.SimplePermissions;
using GCBot.Infrastructure.BotConfiguration;
using GCBot.Services.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Infrastructure
{
    public class GcDatabaseInitContext : GcBotConfig
    {
        private readonly string _connectionString;
        public DbSet<Message> Messages { get; set; }
        public DbSet<AllowedExtension> AllowedExtensions { get; set; }

        public GcDatabaseInitContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {   
            
            optionsBuilder.UseMySql(_connectionString);
        }
    }
}