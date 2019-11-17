using System.Linq;

namespace GCBot.Models.Backup
{
    public class ChannelReport : Report<uint, UserReport>
    {
        public ChannelReport(uint channelId, DateRange dateRange) : base(channelId, dateRange)
        { }

        public override int TotalMessages =>
            Information.Values.Sum(a => a.TotalMessages);
    }
}
