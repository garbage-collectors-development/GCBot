using GCBot.Models.Backup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCBot.Services.Repositories
{
    public interface IBackupRepository
    {
        Task AddMessageAsync(UserMessage message);
        IEnumerable<UserMessage> GetMessagesByUser(DateRange date, ulong id);
        IEnumerable<UserMessage> GetMessagesByChannel(DateRange date, ulong channel);
    }
}
