using Newtonsoft.Json;
using ngov3;
using System;
using System.IO;

namespace PlaythroughLogs
{
    internal class ExportLogs
    {
        internal static LanguageType SetLang = (LanguageType)999;

        internal static string LogFolderPath;

        internal static string currentId;

        internal static string DateOneFile;
        internal static string DateTwoFile;
        internal static string DateThreeFile;
        internal static void CreateLogDirectory()
        {
            string path = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            LogFolderPath = path;
        }

        internal static void SetLangSettings()
        {
            string path = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "settings.json");
            var langSettings = new LogSettings();
            if (!File.Exists(path))
            {
                var set = JsonConvert.SerializeObject(langSettings);
                File.WriteAllText(path, set);
            }
            else
            {
                langSettings = (LogSettings)JsonConvert.DeserializeObject(path);
                if (langSettings.LogLanguage <= LanguageType.SP)
                {
                    SetLang = langSettings.LogLanguage;
                }
            }
        }
        internal static void SetPlaythroughId()
        {
            currentId = $"{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}";
        }
        internal static void SavePlaythroughToJSON()
        {
            string file = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs", $"Log_{currentId}.ame");
            string json = JsonConvert.SerializeObject(DataLogger.currentLog, Formatting.Indented);
            File.WriteAllText(file, json);
        }

        internal static void SaveDataLogsToCSV()
        {
            LanguageType lang = SetLang != (LanguageType)999 ? SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value : SetLang;
            string header = $"Event," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.Day, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.Day, lang)}%," +
                $"Action," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.JINE_Kidoku, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.Stress, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.Follower, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.Love, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.Yami, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.RenzokuHaishinCount, lang)} ," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.SNSzizenKokutiBonus, lang)} ," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.GameCountBonus, lang)} ," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.CinePhillCountBonus, lang)} ," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.OkusuriedCounter, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.OirokeCounter, lang)}," +
                $"{NgoEx.SystemTextFromType(NGO.SystemTextType.DougaTVShabekuriCountBonus, lang)}," +
                $"{NgoEx.SystemTextFromTypeString("Harumagedo", lang)}," +
                $"{NgoEx.ActNameFromType(ActionType.PlayMakeLove, lang)}," +
                $"{JineDataConverter.GetJineTextFromTypeId(NGO.JineType.Event_Menherafriend_JINE002_Option003)}" +
                $"{NgoEx.ActNameFromType(ActionType.OkusuriPsyche, lang)}";
            string dataOne = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs", $"Log1_{currentId}.csv");
            string dataTwo = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs", $"Log2_{currentId}.csv");
            string dataThree = Path.Combine(Path.GetDirectoryName(NGOPlugin.PInfo.Location), "Logs", $"Log3_{currentId}.csv");
            for (int i = 0; i < DataLogger.currentLog.Datas.Count; i++)
            {
                var data = DataLogger.currentLog.Datas[i];
                switch (data.SaveNum)
                {
                    case 1:
                        if (!File.Exists(dataOne))
                        {
                            File.AppendAllText(dataOne, header);
                            File.AppendAllText(dataOne, "Data" + data.SaveNum.ToString());
                        }
                        SaveDayLogsToFile(dataOne, data);
                        break;
                    case 2:
                        if (!File.Exists(dataTwo))
                        {
                            File.AppendAllText(dataTwo, header);
                            File.AppendAllText(dataTwo, "Data" + data.SaveNum.ToString());
                        }
                        SaveDayLogsToFile(dataTwo, data);
                        break;
                    case 3:
                        if (!File.Exists(dataThree))
                        {
                            File.AppendAllText(dataThree, header);
                            File.AppendAllText(dataThree, "Data" + data.SaveNum.ToString());
                        }
                        SaveDayLogsToFile(dataThree, data);
                        break;
                }

            }
        }

        internal static void SaveDayLogsToFile(string filePath, DataInfo data)
        {
            LanguageType lang = SetLang != (LanguageType)999 ? SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value : SetLang;
            for (int i = 0; i < data.Days.Count; i++)
            {
                var day = data.Days[i];
                if (day.startingStats != null)
                {
                    File.AppendAllText(filePath, $"{day.DayEventName},{day.Day},{NgoEx.DayText(0, lang)}," +
                             $"," +
                             $"," +
                             $"{day.endingStats.Followers}," +
                             $"{day.endingStats.Stress}," +
                             $"{day.endingStats.Affection}," +
                             $"{day.endingStats.Darkness}," +
                             $"{day.endingStats.StreamStreak}," +
                             $"{day.endingStats.PreAlertBonus}," +
                             $"{day.endingStats.GamerGirl}," +
                             $"{day.endingStats.Cinephile}," +
                             $"{day.endingStats.Impact}," +
                             $"{day.endingStats.Experience}," +
                             $"{day.endingStats.Communication}, " +
                             $"{day.endingStats.RabbitHole}, " +
                             $"{day.endingStats.LoveCounter}," +
                             $"{day.endingStats.IgnoreCounter}," +
                             $"{day.endingStats.PsycheCounter}");
                }
                else
                {
                    File.AppendAllText(filePath, $"{day.DayEventName},{day.Day},{NgoEx.DayText(0, lang)},,,,,,,,,,,,,,,,,");
                }
                for (int j = 0; j < day.Commands.Count; j++)
                {
                    var cmd = day.Commands[j];
                    if (cmd.Ending != NGO.EndingType.Ending_None)
                    {
                        File.AppendAllText(filePath, $"{NgoEx.EndingFromType(cmd.Ending)},{day.Day},{NgoEx.DayText(0, lang)}," +
                                                         $"," +
                         $"," +
                         $"{day.endingStats.Followers}," +
                         $"{day.endingStats.Stress}," +
                         $"{day.endingStats.Affection}," +
                         $"{day.endingStats.Darkness}," +
                         $"{day.endingStats.StreamStreak}," +
                         $"{day.endingStats.PreAlertBonus}," +
                         $"{day.endingStats.GamerGirl}," +
                         $"{day.endingStats.Cinephile}," +
                         $"{day.endingStats.Impact}," +
                         $"{day.endingStats.Experience}," +
                         $"{day.endingStats.Communication}, " +
                         $"{day.endingStats.RabbitHole}, " +
                         $"{day.endingStats.LoveCounter}," +
                         $"{day.endingStats.IgnoreCounter}," +
                         $"{day.endingStats.PsycheCounter}");
                        return;
                    }
                    else
                    {
                        var param = CmdToParam(cmd.Command);
                        File.AppendAllText(filePath, $",,{NgoEx.DayText(cmd.DayPart, lang)}," +
                                                     $"{CmdToName(cmd.Command, lang)}," +
                                                     $"{!cmd.SkippedDM}," +
                                                     $"{param.FollowerDelta}," +
                                                     $"{param.StressDelta}{(cmd.SkippedDM ? " + 5" : "")}," +
                                                     $"{param.FavorDelta}{(cmd.SkippedDM ? " - 4" : "")}," +
                                                     $"{param.YamiDelta}," +
                                                     $"{(param.ParentAct == "Haishin" ? 1 : 0)}," +
                                                     $"{param.SNS}," +
                                                     $"{param.GameCount}," +
                                                     $"{param.CinePhillCount}," +
                                                     $"{param.OkusuriCount}," +
                                                     $"{param.OirokeCount}," +
                                                     $"{param.ShabekuriCount}, " +
                                                     $"{param.Harumagedo}, " +
                                                     $"{(param.ParentAct == "PlayMakeLove" ? 1 : 0)}," +
                                                     $"{(cmd.SkippedDM ? 1 : 0)}," +
                                                     $"{(param.ParentAct == "OkusuriPsyche" ? 1 : 0)}");
                    }
                }
                File.AppendAllText(filePath, $"{day.MidnightEventName},{day.Day},," +
                                             $"," +
                                             $"," +
                                             $"{day.endingStats.Followers}," +
                                             $"{day.endingStats.Stress}," +
                                             $"{day.endingStats.Affection}," +
                                             $"{day.endingStats.Darkness}," +
                                             $"{day.endingStats.StreamStreak}," +
                                             $"{day.endingStats.PreAlertBonus}," +
                                             $"{day.endingStats.GamerGirl}," +
                                             $"{day.endingStats.Cinephile}," +
                                             $"{day.endingStats.Impact}," +
                                             $"{day.endingStats.Experience}," +
                                             $"{day.endingStats.Communication}, " +
                                             $"{day.endingStats.RabbitHole}, " +
                                             $"{day.endingStats.LoveCounter}," +
                                             $"{day.endingStats.IgnoreCounter}," +
                                             $"{day.endingStats.PsycheCounter}");

            }
        }
        internal static string CmdToName(CmdType type, LanguageType lang)
        {
            if (type == CmdType.OdekakeOdaiba)
            {
                return "MV";

            }
            else if (type == (CmdType)200)
            {
                return "";
            }
            else
            {
                return NgoEx.CmdName(type, lang);
            }
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
