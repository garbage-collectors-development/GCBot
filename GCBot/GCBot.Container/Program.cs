﻿using System;
using System.Threading.Tasks;
using GCBot.Infrastructure;
using GCBot.Services.EntityFramework;
using GCBot.Services.EntityFramework.Repositories;
using GCBot.Services;
using GCBot.Services.Repositories;
using GCBot.Services.Services;
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

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            if (env == "Development")
            {
                builder.AddUserSecrets<Program>();
            }

            IConfigurationRoot config = builder.Build();
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(typeof(IConfigurationRoot), config);

           serviceCollection.AddDbContext<GCContext>(b => b.UseSqlServer(config.GetConnectionString("Database")));
           serviceCollection.AddSingleton<IBackupService, BackupService>()
                .AddSingleton<IBackupRepository, BackupRepository>();
                
            serviceCollection.AddSingleton<IAttachmentService, AttachmentService>()
                .AddSingleton<IAllowedExtensionRepository, AllowedExtensionRepository>();

            var client = new Client(serviceCollection, config);
            await client.RunAsync();
        }
    }
}
