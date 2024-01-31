using System.Collections.Generic;

namespace PlaythroughLogs
{
    public class DayInfo
    {
        public string EventName;
        public int Day;
        public ResultInfo startingStats;
        public ResultInfo endingStats;
        public List<CommandInfo> Commands;

    }
}
