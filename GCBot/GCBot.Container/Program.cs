using System;
using System.Threading.Tasks;
using GCBot.Core;
using GCBot.Core.Services;
using GCBot.EntityFramework.Repositories;
using GCBot.Services;
using GCBot.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GCBot.Container
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IBackupService, BackupService>()
                .AddSingleton<IBackupRepository, BackupRepository>();

            Client client = new Client(serviceCollection);
            await client.RunAsync();
        }
    }
}
