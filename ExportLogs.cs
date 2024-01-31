using Newtonsoft.Json;
using ngov3;
using System;
using System.IO;

namespace PlaythroughLogs
{
    internal class ExportLogs
    {
        internal static string LogFolderPath;

        internal static string DateOneFile;
        internal static string DateTwoFile;
        internal static string DateThreeFile;
        internal void CreateLogDirectory()
        {
            string path = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            LogFolderPath = path;
        }

        internal static void SavePlaythroughToJSON()
        {
            string file = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs", $"Log_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.ame");
            string json = JsonConvert.SerializeObject(DataLogger.currentLog, Formatting.Indented);
            File.WriteAllText(file, json);
        }

        internal static void SaveDataLogsToCSV()
        {

        }
        internal static CmdMaster.Param CmdToParam(CmdType type)
        {
            if (type == CmdType.OdekakeOdaiba)
            {
                return new CmdMaster.Param()
                {
                    PassingTime = 2
                };

            }
            else if (type == (CmdType)200)
            {
                return new CmdMaster.Param()
                {
                    PassingTime = 2,
                    StressDelta = -15,
                    FavorDelta = 6

                };
            }
            else
            {
                return NgoEx.CmdFromType(type);
            }
        }
    }
}
