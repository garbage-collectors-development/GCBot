﻿using System;

namespace GCBot.Shared.Backup
{
    public struct DateRange
    {
        public readonly DateTime BeginDate;
        public readonly DateTime EndDate;

        public DateRange(DateTime beginDate, DateTime endDate)
        {
            BeginDate = beginDate;
            EndDate = endDate;
        }
    }
}
