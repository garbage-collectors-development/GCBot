using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace GCBot.Core
{
    public class Client
    {
        public IServiceProvider Services { get; }
        public DiscordSocketClient SocketClient = new DiscordSocketClient();

        // locals
        private readonly CommandService _commands = new CommandService();
        public Client(ServiceCollection serviceDescriptors)
        {
            if (serviceDescriptors == null) serviceDescriptors = new ServiceCollection();

            Services = serviceDescriptors
                .AddSingleton(SocketClient)
                .AddSingleton(_commands)
                .BuildServiceProvider();
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

                var token = await File.ReadAllTextAsync("/home/julian/bot/token.txt"); // /home/julian/GCBot/token.txt
                await SocketClient.LoginAsync(Discord.TokenType.Bot, token.Trim());
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
