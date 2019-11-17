using System;
using System.Collections.Generic;

namespace GCBot.Models.Backup
{
    public class UserHistory : UserReport
    {
        public UserHistory(uint userId, DateRange dateRange) : base(userId, dateRange)
        {
            Messages = new Dictionary<DateTime, UserMessage>();
        }

        public IDictionary<DateTime, UserMessage> Messages { get; set; }
    }
}
