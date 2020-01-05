using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCBot.Services.Repositories;
using GCBot.Models.Backup;
using GCBot.Services.Services;

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

        public ChannelReport GenerateChannelReport(ulong channelId, DateRange dateRange) =>
             new ChannelReport(channelId, dateRange)
                {
                    DateRange = dateRange,
                    DiscordUnitId = channelId,
                    Information = GenerateUserReports(dateRange)
                };
        
        public DiscordReport GenerateDiscordReport(DateRange dateRange) =>
            new DiscordReport(dateRange)
            {
                DateRange = dateRange,
                Information = GenerateChannelReports(dateRange),
            };

        public UserReport GenerateUserReport(ulong userId, DateRange dateRange)
        {
            var userReport = new UserReport(userId,dateRange)
            {
                Information = new Dictionary<DateTime, int>()
            };

            var day = dateRange.BeginDate;

            while (day.Date < dateRange.EndDate.Date)
            {
                var count = _backupRepository.GetNumberOfMessagesByUser(day, userId);
                userReport.Information.Add(day, count);
                day = day.AddDays(1);
            }
            return userReport;
        }

        private Dictionary<ulong, UserReport> GenerateUserReports(DateRange dateRange)
        {
            var userIds = _backupRepository.GetAllUserIds(dateRange);

            var dict = new Dictionary<ulong, UserReport>();

            while (userIds.GetEnumerator().MoveNext())
            {
                var userId = userIds.GetEnumerator().Current;
                dict.Add(userId, GenerateUserReport(userId, dateRange));
            }

            return dict;
        }

        private Dictionary<ulong, ChannelReport> GenerateChannelReports(DateRange dateRange)
        {
            var channelIds = _backupRepository.GetAllChannelIds(dateRange);

            var dict = new Dictionary<ulong, ChannelReport>();

            while (channelIds.GetEnumerator().MoveNext())
            {
                var channelId = channelIds.GetEnumerator().Current;
                dict.Add(channelId, GenerateChannelReport(channelId,dateRange));
            }

            return dict;
        }
    }
}
