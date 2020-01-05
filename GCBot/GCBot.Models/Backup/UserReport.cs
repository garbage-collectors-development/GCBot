using System;
using System.Linq;

namespace GCBot.Models.Backup
{
    public class UserReport : Report<DateTime, int>
    {
        public UserReport(ulong userId, DateRange dateRange) : base(dateRange,userId) { }

        public override int TotalMessages =>
            Information.Values.Sum(a => a);

        public string UserName { get; set; }
    }
}
