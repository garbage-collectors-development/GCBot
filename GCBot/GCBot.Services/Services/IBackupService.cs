using System.Collections.Generic;
using System.Threading.Tasks;
using GCBot.Models.Backup;

namespace GCBot.Services.Services
{
    public interface IBackupService
    {
        Task BackupMessageAsync(UserMessage userMessage);
        Task BackupMessagesAsync(IEnumerable<UserMessage> userMessages);

        DiscordReport GenerateDiscordReport(DateRange dateRange);
        ChannelReport GenerateChannelReport(ulong channelId, DateRange dateRange);
        UserReport GenerateUserReport(ulong userId, DateRange dateRange);
    }
}
