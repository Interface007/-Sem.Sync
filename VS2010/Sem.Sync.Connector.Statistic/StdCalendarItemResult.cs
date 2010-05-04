using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.Sync.Connector.Statistic
{
    using Sem.Sync.SyncBase;

    public class StdCalendarItemResult
    {
        public int NextFourteenDays { get; set; }

        public int NextSevenDays { get; set; }

        public static StdCalendarItemResult GetStdCalendarItemResult(List<StdCalendarItem> calendarItems)
        {
            if (calendarItems == null)
            {
                return null;
            }

            if (calendarItems.Count <= 0)
            {
                return null;
            }

            return new StdCalendarItemResult
                        {
                            NextSevenDays = (from x in calendarItems
                                             where x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(7)
                                             select x).Count(),
                            NextFourteenDays = (from x in calendarItems
                                                where x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(14)
                                                select x).Count()
                        };
        }

        
    }
}
