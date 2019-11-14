using System;
using System.Linq;

namespace GCBot.Shared.Backup
{
    public class UserReport : Report<DateTime, int>
    {
        public UserReport(uint userId, DateRange dateRange) : base(userId, dateRange) { }

        public override int TotalMessages =>
            Information.Values.Sum(a => a);
    }
}
