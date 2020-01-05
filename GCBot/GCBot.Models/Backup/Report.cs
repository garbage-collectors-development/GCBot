using System.Collections.Generic;

namespace GCBot.Models.Backup
{
    public abstract class Report<T, TS>
    {
        protected Report(DateRange dateRange, uint discordUnitId = 0)
        {
            DiscordUnitId = discordUnitId;
            DateRange = dateRange;
            Information = new Dictionary<T, TS>();
        }

        // e.g. discord ID/channel ID/user ID
        public uint DiscordUnitId { get; set; }

        public DateRange DateRange { get; set; }

        public Dictionary<T, TS> Information { get; set; }

        public abstract int TotalMessages { get; }
    }
}
