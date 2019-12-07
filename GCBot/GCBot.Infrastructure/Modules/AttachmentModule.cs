using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.SimplePermissions;
using Discord.Commands;
using GCBot.Services.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace GCBot.Infrastructure.Modules
{
    [Group("ext")]
    [Summary("Commands used to manage allowed message attachment extensions.")]
    [Name("Allowed Extension Management")]
    public class AttachmentModule : ModuleBase<SocketCommandContext>
    {
        private readonly IAttachmentService _service;
        private readonly CommandService _commandService;

        public AttachmentModule(IAttachmentService service, CommandService commandService)
        {
            _service = service;
            _commandService = commandService;
        }

        [Command]
        // This attribute makes it so the command doesn't appear in the help list
        [Name("")]
        [Permission(MinimumPermission.Everyone)]
        public async Task PrintCommands()
        {
            ModuleInfo module = _commandService.Modules.FirstOrDefault(mi => mi.Name == ((NameAttribute) this.GetType().GetCustomAttribute(typeof(NameAttribute))).Text);

            if (module == null)
            {
                await ReplyAsync("Error retrieving help list");
                return;
            }
            
            var commands = module.Commands.Where(c => c.Name.Length > 0).ToList();
            
            EmbedBuilder builder = new EmbedBuilder()
                .WithFooter(footerBuilder => footerBuilder
                    .WithIconUrl("https://cdn.discordapp.com/icons/573892843647664140/a617f685df3613530512d72019799272.png?size=32")
                    .WithText("© Garbage Collectors"))
                .WithCurrentTimestamp()
                .WithAuthor(Context.Client.CurrentUser.Username, "https://cdn.discordapp.com/icons/573892843647664140/a617f685df3613530512d72019799272.png?size=32")
                .WithTitle(module.Name)
                .WithDescription(module.Summary);
            
            foreach (CommandInfo command in commands)
            {
                string parameterList = command.Parameters.Select(p => p.IsOptional ? $"[${p}]" : $"{{{p.Name}}}").Join(" ");

                string commandStr = "$" + (module.Group.Length > 0 ? $"{module.Group} " : "") + $"{command.Name} {parameterList}";

                string summary = command.Summary?.Length > 0 ? command.Summary : "_No Description_";

                builder.AddField(commandStr, summary);
            }
            
            await ReplyAsync(embed: builder.Build());
        }
        
        [Command("add")]
        [Alias("whitelist")]
        [Summary("Allows files with this extension")]
        [Permission(MinimumPermission.ModRole)]
        public async Task AddExtension(string extension)
        {
            _service.WhitelistExtension(extension, Context.User.Id);
            await ReplyAsync($"Files with the extension `{extension}` can now be posted by anyone.");
        }

        [Command("remove")]
        [Alias("rem", "blacklist")]
        [Summary("Disallows files with this extension")]
        [Permission(MinimumPermission.ModRole)]
        public async Task RemoveExtension(string extension)
        {
            _service.BlacklistExtension(extension);
            await ReplyAsync($"Files with the extension `{extension}` are now blacklisted from being posted.");
        }

        [Command("list")]
        [Alias("all")]
        [Summary("Show all allowed file extensions")]
        [Permission(MinimumPermission.Everyone)]
        public async Task ListExtensions()
        {
            var extensions = _service.GetAllAllowedExtensions();
            await ReplyAsync($"Message attachments with the following extensions are allowed: `{string.Join(", ",extensions.Select(e => e.Value))}`");
        }
    }
}