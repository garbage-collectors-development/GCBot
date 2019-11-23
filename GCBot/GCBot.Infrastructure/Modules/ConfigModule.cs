using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Addons.SimplePermissions;
using Discord.Commands;
using Discord.WebSocket;
using GCBot.Infrastructure.BotConfiguration;

namespace GCBot.Infrastructure.Modules
{
    public class ConfigModule : ModuleBase
    {
        private readonly IConfigStore<GcBotConfig> _configStore;

        public ConfigModule(IConfigStore<GcBotConfig> configStore)
        {
            _configStore = configStore;
        }
        
        [Command("SetPrefix")]
        [Permission(MinimumPermission.AdminRole)]
        public async Task SetPrefix(char prefix)
        {
            if (char.IsLetterOrDigit(prefix) || char.IsWhiteSpace(prefix))
            {
                await ReplyAsync(
                    $"`{prefix}` is not a valid command prefix. " +
                    $"Letters, numbers, and whitespaces are not acceptable as a command prefix.");
                return;
            };
            
            using (GcBotConfig config = _configStore.Load())
            {
                GcGuild guild = config.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id);
                if (guild == null) return;

                guild.CommandPrefix = prefix;
                config.Save();

                await ReplyAsync($"Updated the bot command prefix to {prefix}");
            }
        }
    }
}