using GCBot.Shared.Backup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCBot.Core.Services
{
    public interface IBackupService
    {
        Task BackupMessageAsync(UserMessage userMessage);
        Task BackupMessagesAsync(IEnumerable<UserMessage> userMessages);

        DiscordReport GenerateDiscordReport(DateRange dateRange);
        ChannelReport GenerateChannelReport(uint channelId, DateRange dateRange);
        UserReport GenerateUserReport(uint userId, DateRange dateRange);
    }
}
