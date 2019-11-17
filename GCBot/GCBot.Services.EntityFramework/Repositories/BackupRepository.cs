using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCBot.Services.EntityFramework.Entities;
using GCBot.Services.Repositories;
using GCBot.Models.Backup;

namespace GCBot.Services.EntityFramework.Repositories
{
    public class BackupRepository : IBackupRepository
    {
        private readonly BackupContext _context;

        public BackupRepository(BackupContext context)
        {
            _context = context;
        }

        public IEnumerable<UserMessage> GetMessagesByUser(DateRange date, ulong userId)
        {
            var msg = _context.Messages.Where(x => x.SenderId == userId &&  x.DateSent.Date >= date.BeginDate.Date && x.DateSent.Date <= date.EndDate.Date);
            return msg.Select(item => new UserMessage()
            {
                Id = item.Id, Link = item.Link, SenderId = item.SenderId, Text = item.Text, DateSent = item.DateSent,
                ChannelId = item.ChannelId
            });
        }

        public IEnumerable<UserMessage> GetMessagesByChannel(DateRange date, ulong channel)
        {
            var msg = _context.Messages.Where(x => x.ChannelId == channel && x.DateSent.Date >= date.BeginDate.Date && x.DateSent.Date <= date.EndDate.Date);

            return msg.Select(item => new UserMessage()
            {
                Id = item.Id,
                Link = item.Link,
                SenderId = item.SenderId,
                Text = item.Text,
                DateSent = item.DateSent,
                ChannelId = item.ChannelId
            });
        }

        public async Task AddMessageAsync(UserMessage message)
        {
            await _context.Messages.AddAsync(new Message()
            {
                Id = message.Id,
                Link = message.Link,
                SenderId = message.SenderId,
                ChannelId = message.ChannelId,
                Text = message.Text,
                DateSent = message.DateSent
            });

            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
        }
    }
}
