using NGO;

namespace PlaythroughLogs
{
    public class EndingInfo : CommandInfo
    {
        public EndingType Ending;
        public new bool SkippedDM = false;
        public new CmdMaster.Param Param = null;
    }
}
