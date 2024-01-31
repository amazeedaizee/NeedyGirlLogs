using System;
using System.Collections.Generic;

namespace PlaythroughLogs
{
    [Serializable]
    public class DataInfo
    {
        public int SaveNum = 0;
        public List<DayInfo> Days = new List<DayInfo>();
    }
}
