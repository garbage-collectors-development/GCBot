using Discord.Addons.SimplePermissions;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Infrastructure.BotConfiguration
{
    public sealed class GcUser : ConfigUser {}
    public sealed class GcChannel : ConfigChannel<GcUser> {}

    public sealed class GcGuild : ConfigGuild<GcChannel, GcUser>
    {
        public char CommandPrefix { get; set; } = Client.DefaultCommandPrefix;
    }
    public class GcBotConfig : EFBaseConfigContext<GcGuild, GcChannel, GcUser>
    {
        public GcBotConfig() : base(new DbContextOptions<GcBotConfig>())
        {
        }

        public GcBotConfig(DbContextOptions options) : base(options)
        {
        }
    }
}