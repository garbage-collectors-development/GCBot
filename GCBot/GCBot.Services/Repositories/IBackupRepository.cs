using System;
using GCBot.Models.Backup;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GCBot.Services.Repositories
{
    public interface IBackupRepository
    {
        Task AddMessageAsync(UserMessage message);
        IQueryable<UserMessage> GetMessagesByUser(DateRange range, ulong id);
        IQueryable<UserMessage> GetMessagesByChannel(DateRange range, ulong channel);

        int GetNumberOfMessagesByUser(DateTime date, ulong id);
        int GetNumberOfMessagesByChannel(DateTime date, ulong channel);

        IQueryable<ulong> GetAllUserIds(DateRange range);
        IQueryable<ulong> GetAllChannelIds(DateRange range);

    }
}
