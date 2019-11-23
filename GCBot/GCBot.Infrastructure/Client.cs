using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.SimplePermissions;
using Discord.Commands;
using Discord.WebSocket;
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

        public IServiceProvider Services { get; }
        public DiscordSocketClient SocketClient;

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
            
            SocketClient = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = logLevel});

            Services = serviceDescriptors
                .AddSingleton(SocketClient)
                .AddSingleton(_commands)
                .AddSingleton(new PermissionsService(_botConfigStore, _commands, SocketClient, Log))
                .AddSingleton(_botConfigStore)
                .AddDbContext<GcBotConfig>(options => options.UseMySql(_applicationConfiguration.GetConnectionString("Database")))
                .BuildServiceProvider();

            Services.GetService<GcBotConfig>().Database.EnsureCreated();
            ((EFConfigStore<GcBotConfig, GcGuild, GcChannel, GcUser>) _botConfigStore).Services = Services;
        }

        /// <summary>
        /// Executes the startup sequence
        /// </summary>
        public async Task RunAsync()
        {
            try
            {
                SocketClient.Log += Log;
                await RegisterCommandsAsync();

                await SocketClient.LoginAsync(Discord.TokenType.Bot, _applicationConfiguration["token"]);
                await SocketClient.StartAsync();

                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task RegisterCommandsAsync()
        {
            SocketClient.MessageReceived += HandleCommandAsync;
            await _commands.AddModuleAsync<PermissionsModule>(Services);
            await _commands.AddModulesAsync(GetType().Assembly, Services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            int argPos = 0;

            if (msg is null) return;

            using (GcBotConfig config = _botConfigStore.Load())
            {
                SocketGuildUser user = msg.Author as SocketGuildUser;

                char commandPrefix = DefaultCommandPrefix;

                if (user != null && config != null)
                {
                    GcGuild guild = config.Guilds.FirstOrDefault(g => g.GuildId == user.Guild.Id);
                    if (guild != null)
                    {
                        commandPrefix = guild.CommandPrefix;
                    }
                }
                
                if (!msg.Author.IsBot && msg.HasCharPrefix(commandPrefix, ref argPos))
                {
                    var context = new SocketCommandContext(SocketClient, msg);
                    var result = await _commands.ExecuteAsync(context, argPos, Services);
                 
                    if (!result.IsSuccess) return;
                }
            }

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}
