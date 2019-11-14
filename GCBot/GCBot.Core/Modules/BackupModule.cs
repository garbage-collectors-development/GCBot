using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GCBot.Core.Services;
using GCBot.Shared.Backup;

namespace GCBot.Core.Modules
{
    public class BackupModule : ModuleBase
    {
        private readonly IBackupService _service;

        public BackupModule(IBackupService service)
        {
            _service = service;
        }

        [Command("test")]
        public async Task Test()
        {
            await ReplyAsync("test");
        }

        [Command("DiscordReport")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task ShowDiscordReport([Remainder] string remainder)
        {
            string[] args = remainder.Split(' ');

            try
            {
                var range = GetRange(args);
                var report = _service.GenerateDiscordReport(range);
            }
            catch (ArgumentNullException e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("ChannelReport")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task ShowChannelReport([Remainder] string remainder)
        {
            string[] args = remainder.Split(' ');

            if (args.Length < 1) return;

            try
            {
                var range = GetRange(args.Length == 2 ? new[] { args[1] } : new[] { args[1], args[2] }); // haha, sorry I am a bit lazy I admit..
                var isParsed = uint.TryParse(args[0], out var id);
                if (!isParsed)
                {
                    await ReplyAsync("Channel ID is not an unsigned integer.");
                    return;
                }
                var report = _service.GenerateChannelReport(id, range);
            }
            catch (ArgumentNullException e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("UserReport")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task ShowUserReport([Remainder] string remainder)
        {
            string[] args = remainder.Split(' ');

            if (args.Length < 1) return;

            try
            {
                var range = GetRange(args.Length == 2 ? new[] { args[1] } : new[] { args[1], args[2] }); 
                var isParsed = uint.TryParse(args[0], out var id);
                if (!isParsed)
                {
                    await ReplyAsync("Channel ID is not an unsigned integer.");
                    return;
                }
                var report = _service.GenerateUserReport(id, range);
            }
            catch (ArgumentNullException e)
            {
                await ReplyAsync(e.Message);
            }
        }

        private DateRange GetRange(string[] args)
        {
            DateRange range = new DateRange(DateTime.MinValue, DateTime.Now);

            switch (args.Length)
            {
                case 1:
                {
                    var isParsed = DateTime.TryParse(args[0], out var date);
                    if (!isParsed)
                    {
                        throw new ArgumentNullException($"{date} not parsed successfully to DateTime");
                    }
                    range = new DateRange(date, date);
                    break;
                }

                case 2:
                {
                    var isBeginTimeParsed = DateTime.TryParse(args[0], out var beginDate);
                    var isEndTimeParsed = DateTime.TryParse(args[1], out var endDate);

                    if (!isBeginTimeParsed || !isEndTimeParsed)
                    {
                        throw new ArgumentNullException($"{beginDate} or {endDate} not parsed successfully to DateTime");
                    }
                    range = new DateRange(beginDate, endDate);
                    break;
                }
            }

            return range;
        }

    }
}
