using System.Collections.Generic;

namespace GCBot.Models.Backup
{
    public abstract class Report<T, TS>
    {
        protected Report(DateRange dateRange, ulong discordUnitId = 0)
        {
            DiscordUnitId = discordUnitId;
            DateRange = dateRange;
            Information = new Dictionary<T, TS>();
        }

        // e.g. discord ID/channel ID/user ID
        public ulong DiscordUnitId { get; set; }

        public DateRange DateRange { get; set; }

        public Dictionary<T, TS> Information { get; set; }

        public abstract int TotalMessages { get; }
    }
}
