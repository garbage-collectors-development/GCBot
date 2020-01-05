using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GCBot.Services.EntityFramework.Entities;
using GCBot.Services.Repositories;
using GCBot.Models.Backup;
using Microsoft.EntityFrameworkCore;

namespace GCBot.Services.EntityFramework.Repositories
{
    public class BackupRepository : IBackupRepository
    {
        private readonly GCContext _context;

        public BackupRepository(GCContext context)
        {
            _context = context;
        }

        public IQueryable<Message> Get(
            Expression<Func<Message, bool>> filter = null,
            Func<IQueryable<Message>, IOrderedQueryable<Message>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Message> query = _context.Messages.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query) : query;
        }

        public int GetNumberOfMessagesByUser(DateTime date, ulong id) =>
            _context.Messages.Count(t => t.SenderId == id && t.DateSent.Date == date.Date);

        public int GetNumberOfMessagesByChannel(DateTime date, ulong channel) =>
            _context.Messages.Count(t => t.ChannelId == channel && t.DateSent.Date == date.Date);

        public IQueryable<ulong> GetAllUserIds(DateRange range) =>
            _context.Messages.Select(msg => msg.SenderId).Distinct();

        public IQueryable<ulong> GetAllChannelIds(DateRange range) =>
            _context.Messages.Select(msg => msg.ChannelId).Distinct();

        public IQueryable<UserMessage> GetMessagesByUser(DateRange date, ulong userId)
        {
            var msg = _context.Messages.Where(x => x.SenderId == userId &&  x.DateSent.Date >= date.BeginDate.Date && x.DateSent.Date <= date.EndDate.Date);
            return msg.Select(item => new UserMessage()
            {
                Link = item.Link, SenderId = item.SenderId, Text = item.Text, DateSent = item.DateSent,
                ChannelId = item.ChannelId
            });
        }

        public IQueryable<UserMessage> GetMessagesByChannel(DateRange date, ulong channel)
        {
            var msg = _context.Messages.Where(x => x.ChannelId == channel && x.DateSent.Date >= date.BeginDate.Date && x.DateSent.Date <= date.EndDate.Date);

            return msg.Select(item => new UserMessage()
            {
                Link = item.Link,
                SenderId = item.SenderId,
                Text = item.Text,
                DateSent = item.DateSent,
                ChannelId = item.ChannelId
            });
        }

        public int GetNumberOfMessagesByUser(DateRange range, ulong id)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfMessagesByChannel(DateRange range, ulong channel)
        {
            throw new NotImplementedException();
        }

        public async Task AddMessageAsync(UserMessage message)
        {
            await _context.Messages.AddAsync(new Message()
            {
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
                Console.WriteLine(e.InnerException); // have to add a logger here
            }
        }

    }
}
