using System;
using GCBot.Models.Backup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCBot.Services.Repositories
{
    public interface IBackupRepository
    {
        Task AddMessageAsync(UserMessage message);
        IEnumerable<UserMessage> GetMessagesByUser(DateRange range, ulong id);
        IEnumerable<UserMessage> GetMessagesByChannel(DateRange range, ulong channel);

        int GetNumberOfMessagesByUser(DateTime date, ulong id);
        int GetNumberOfMessagesByChannel(DateTime date, ulong channel);

        IEnumerable<uint> GetAllUserIds(DateRange range);
        IEnumerable<uint> GetAllChannelIds(DateRange range);

    }
}
