using System;
using System.Threading.Tasks;
using GCBot.Core;
using GCBot.Core.Services;
using GCBot.EntityFramework;
using GCBot.EntityFramework.Repositories;
using GCBot.Services;
using GCBot.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GCBot.Container
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ENVIRONMENT");
            
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(typeof(IConfiguration), config);
            
            serviceCollection.AddSingleton<IBackupService, BackupService>()
                .AddSingleton<IBackupRepository, BackupRepository>()
                .AddSingleton(new BackupContext(""));

            Client client = new Client(serviceCollection);
            await client.RunAsync();
        }
    }
}
