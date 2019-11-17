using GCBot.Shared.Backup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCBot.Services.Repositories
{
    public interface IBackupRepository
    {
        Task AddMessageAsync(UserMessage message);
        IEnumerable<UserMessage> GetMessagesByUser(DateRange range, ulong id);
        IEnumerable<UserMessage> GetMessagesByChannel(DateRange range, ulong channel);
        int GetNumberOfMessagesByUser(DateRange range, ulong id);
        int GetNumberOfMessagesByChannel(DateRange range, ulong channel);
    }
}
