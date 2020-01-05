using System.Linq;

namespace GCBot.Models.Backup
{
    public class ChannelReport : Report<ulong, UserReport>
    {
        public ChannelReport(ulong channelId, DateRange dateRange) : base(dateRange, channelId)
        { }

        public override int TotalMessages =>
            Information.Values.Sum(a => a.TotalMessages);
    }
}
