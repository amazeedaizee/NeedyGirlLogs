using System;
using System.Collections.Generic;

namespace PlaythroughLogs
{
    [Serializable]
    public class PlaythroughLog
    {
        public List<DataInfo> DataOnes = new List<DataInfo>();
        public List<DataInfo> DataTwos = new List<DataInfo>();
        public List<DataInfo> DataThrees = new List<DataInfo>();
    }
}
