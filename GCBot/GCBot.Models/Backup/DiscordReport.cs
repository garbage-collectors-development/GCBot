using System.Linq;

namespace GCBot.Models.Backup
{
    public class DiscordReport : Report<ulong, ChannelReport>
    {
        public DiscordReport(DateRange dateRange) : base(dateRange) { }

        public override int TotalMessages =>
            Information.Values.Sum(a => a.TotalMessages);

    }
}
