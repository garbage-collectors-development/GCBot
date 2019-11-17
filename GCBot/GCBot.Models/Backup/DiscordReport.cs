using System.Linq;

namespace GCBot.Models.Backup
{
    public class DiscordReport : Report<uint, ChannelReport>
    {
        public DiscordReport(DateRange dateRange) : base(0, dateRange) { }

        public override int TotalMessages =>
            Information.Values.Sum(a => a.TotalMessages);

    }
}
