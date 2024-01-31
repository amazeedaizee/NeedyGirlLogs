using NGO;
using ngov3;
using System;

namespace PlaythroughLogs
{
    [Serializable]
    public class CommandInfo
    {
        public EndingType Ending = EndingType.Ending_None;
        public int DayPart = 0;
        public bool SkippedDM = false;
        public CmdType Command = CmdType.None;
    }
}
