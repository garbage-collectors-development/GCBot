using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GCBot.Infrastructure
{
    public class Client
    {
        public IServiceProvider Services { get; }
        public DiscordSocketClient SocketClient;

        private IConfigurationRoot _configuration;
        
        // locals
        private readonly CommandService _commands = new CommandService();
        public Client(ServiceCollection serviceDescriptors, IConfigurationRoot configuration)
        {
            if (serviceDescriptors == null) serviceDescriptors = new ServiceCollection();
            _configuration = configuration;
            
            if (!Enum.TryParse(_configuration["Logging:LogLevel:Default"], out LogSeverity logLevel))
            {
                logLevel = LogSeverity.Info;
            }
            
            SocketClient = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = logLevel});

            Services = serviceDescriptors
                .AddSingleton(SocketClient)
                .AddSingleton(_commands)
                .BuildServiceProvider();
        }

        public async Task RunAsync()
        {
            try
            {
                SocketClient.Log += Log;
                await RegisterCommandsAsync();

                await SocketClient.LoginAsync(Discord.TokenType.Bot, _configuration["token"]);
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
            await _commands.AddModulesAsync(GetType().Assembly, Services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            int argPos = 0;

            if (msg is null) return;


            if (!msg.Author.IsBot && msg.HasCharPrefix('$', ref argPos))
            {
                var context = new SocketCommandContext(SocketClient, msg);
                var result = await _commands.ExecuteAsync(context, argPos, Services);
             
                if (!result.IsSuccess) return;
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}
