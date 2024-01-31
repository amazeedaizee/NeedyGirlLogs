using System;
using System.Collections.Generic;

namespace PlaythroughLogs
{
    [Serializable]
    public class DayInfo
    {
        public string DayEventName;
        public string MidnightEventName = null;
        public int Day;
        public ResultInfo startingStats = null;
        public ResultInfo endingStats;
        public List<CommandInfo> Commands;

    }
}
