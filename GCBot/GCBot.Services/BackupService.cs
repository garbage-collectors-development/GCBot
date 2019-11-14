using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCBot.Core.Services;
using GCBot.Services.Repositories;
using GCBot.Shared.Backup;

namespace GCBot.Services
{
    public class BackupService : IBackupService
    {
        private readonly IBackupRepository _backupRepository;

        public BackupService(IBackupRepository repository)
        {
            _backupRepository = repository;
        }

        public async Task BackupMessageAsync(UserMessage userMessage)
        {
            var message = new UserMessage()
            {
                Id = userMessage.Id,
                ChannelId = userMessage.ChannelId,
                DateSent = userMessage.DateSent,
                Link = userMessage.Link,
                SenderId = userMessage.SenderId,
                Text = userMessage.Text
            };
            await _backupRepository.AddMessageAsync(message);
        }
        public async Task BackupMessagesAsync(IEnumerable<UserMessage> userMessages)
        {
            while (userMessages.GetEnumerator().MoveNext())
            {
                await BackupMessageAsync(userMessages.GetEnumerator().Current);
            }
        }

        public ChannelReport GenerateChannelReport(uint channelId, DateRange dateRange) =>
             new ChannelReport(channelId, dateRange)
                {
                    DateRange = dateRange,
                    DiscordUnitId = channelId,
                    Information = GenerateUserReports(dateRange)
                };

        public DiscordReport GenerateDiscordReport(uint discordId, DateRange dateRange) =>
            new DiscordReport(dateRange)
            {
                DateRange = dateRange,
                DiscordUnitId = discordId,
                Information = GenerateChannelReports(dateRange)
            };

        public DiscordReport GenerateDiscordReport(DateRange dateRange) =>
            GenerateDiscordReport(0, dateRange); // this should be the default discord ID

        public UserReport GenerateUserReport(uint userId, DateRange dateRange)
        {
            throw new NotImplementedException();

        }

        private Dictionary<uint, UserReport> GenerateUserReports(DateRange dateRange)
        {
            throw new NotImplementedException();
        }

        private Dictionary<uint, ChannelReport> GenerateChannelReports(DateRange dateRange)
        {
            throw new NotImplementedException();
        }
    }
}
