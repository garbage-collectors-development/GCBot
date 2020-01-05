using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.SimplePermissions;
using Discord.Commands;
using Discord.WebSocket;
using GCBot.Models;
using GCBot.Services.Services;
using GCBot.Infrastructure.BotConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GCBot.Infrastructure
{
    public class Client
    {
        public const char DefaultCommandPrefix = '$';

        private readonly DiscordSocketClient _socketClient;
        private readonly MessageHandler _messageHandler; 
        private readonly IServiceProvider _services;
        private readonly IConfigStore<GcBotConfig> _botConfigStore;
        private readonly IConfigurationRoot _applicationConfiguration;
        // locals
        private readonly CommandService _commands = new CommandService();

        public Client(ServiceCollection serviceDescriptors, IConfigurationRoot applicationConfiguration)
        {
            if (serviceDescriptors == null) serviceDescriptors = new ServiceCollection();
            _applicationConfiguration = applicationConfiguration;
            
            if (!Enum.TryParse(_applicationConfiguration["Logging:LogLevel:Default"], out LogSeverity logLevel))
            {
                logLevel = LogSeverity.Info;
            }

            _botConfigStore = new EFConfigStore<GcBotConfig, GcGuild, GcChannel, GcUser>(_commands, Log);
            
            _socketClient = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = logLevel});

            _services = serviceDescriptors
                .AddSingleton(_socketClient)
                .AddSingleton(_commands)
                .AddSingleton(new PermissionsService(_botConfigStore, _commands, _socketClient, Log))
                .AddSingleton(_botConfigStore)
                .AddDbContext<GcBotConfig>(options => options.UseMySql(_applicationConfiguration.GetConnectionString("Database")))
                .BuildServiceProvider();
            
            _messageHandler = new MessageHandler(_services, _socketClient, _commands, _botConfigStore);
            ((EFConfigStore<GcBotConfig, GcGuild, GcChannel, GcUser>) _botConfigStore).Services = _services;
            
            // The uber-context is how we can create the database using a single database with multiple contexts
            // It is only used for database creation and not mean to be injected or referenced
            new GcDatabaseInitContext(_applicationConfiguration.GetConnectionString("Database")).Database.EnsureCreated();
        }

        public async Task RunAsync()
        {
            try
            {
                _socketClient.Log += Log;
                await _messageHandler.RegisterCommandsAsync();

                await _socketClient.LoginAsync(Discord.TokenType.Bot, _applicationConfiguration["token"]);
                await _socketClient.StartAsync();
                
                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }

}
