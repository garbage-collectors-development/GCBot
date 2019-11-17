using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Addons.SimplePermissions;
using Discord.Commands;
using Discord.WebSocket;
using GCBot.Infrastructure.BotConfiguration;
using GCBot.Infrastructure.Extensions;
using GCBot.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GCBot.Infrastructure
{
    public class MessageHandler
    {
        private readonly IAttachmentService _attachmentService;
        private readonly DiscordSocketClient _socketClient;
        private readonly CommandService _commandService;
        private readonly IConfigStore<GcBotConfig> _configStore;

        private readonly IServiceProvider _services;
        
        public MessageHandler(IServiceProvider services, DiscordSocketClient socketClient, CommandService commandService, IConfigStore<GcBotConfig> configStore)
        {
            _services = services;
            _socketClient = socketClient;
            _commandService = commandService;
            _configStore = configStore;
            _attachmentService = services.GetService<IAttachmentService>();
        }   
        
        public async Task RegisterCommandsAsync()
        {
            _socketClient.MessageReceived += HandleCommandAsync;

            await _commandService.AddModuleAsync<PermissionsModule>(_services);
            await _commandService.AddModulesAsync(GetType().Assembly, _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            SocketUserMessage msg = arg as SocketUserMessage;
            int argPos = 0;

            if (msg is null || msg.Author.IsBot) return;

            await ValidateMessageAttachments(msg); 

            using(GcBotConfig config = _configStore.Load())
            {
                SocketGuildUser user = msg.Author as SocketGuildUser;

                char commandPrefix = Client.DefaultCommandPrefix;
                if (user != null && config != null)
                {
                    GcGuild guild = config.Guilds.FirstOrDefault(g => g.GuildId == user.Guild.Id);
                    if (guild != null)
                    {
                        commandPrefix = guild.CommandPrefix;
                    }
                }
                
                if (msg.HasCharPrefix(commandPrefix, ref argPos))
                {
                    SocketCommandContext context = new SocketCommandContext(_socketClient, msg);
                    IResult result = await _commandService.ExecuteAsync(context, argPos, _services);
         
                    if (!result.IsSuccess) return;
                }   
            }
        }

        private async Task ValidateMessageAttachments(SocketMessage msg)
        {
            if (msg.ContainsIllegalExtension(_attachmentService.GetAllAllowedExtensions()))
            {
                await msg.DeleteAsync();
                await msg.Channel.SendMessageAsync("Deleted Message");
            }
        }
    }
}