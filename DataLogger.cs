using HarmonyLib;
using ngov3;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PlaythroughLogs
{
    [HarmonyPatch]
    internal class DataLogger
    {
        internal static bool isFirstLoad = true;
        internal static PlaythroughLog currentLog = new PlaythroughLog();

        internal static DataInfo currentData = new DataInfo();
        internal static DayInfo currentDay = new DayInfo();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), "Load")]
        [HarmonyPatch(typeof(EventManager), "StartOver")]
        public static void SetDataInfo()
        {
            NGOPlugin.logger.LogInfo("This is loading!");
            int saveNum = SingletonMonoBehaviour<Settings>.Instance.saveNumber;
            AddDataToCurrentLog();
            SetDataInfo(saveNum);


        }

        public static void AddDataToCurrentLog()
        {
            if (currentData.Days.Count > 0)
            {
                switch (currentData.SaveNum)
                {
                    case 1:
                        currentLog.DataOnes.Add(currentData);
                        break;
                    case 2:
                        currentLog.DataTwos.Add(currentData);
                        break;
                    case 3:
                        currentLog.DataThrees.Add(currentData);
                        break;
                }

            }
        }

        public static void SetDataInfo(int saveNum)
        {
            currentData = new DataInfo
            {
                SaveNum = saveNum
            };
            currentDay = new DayInfo();
            SaveStartingResult();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), nameof(EventManager.Save))]
        public static void SaveDayToData(int day)
        {
            NGOPlugin.logger.LogInfo("This is saving!");
            currentDay.Day = day - 1;
            if (currentDay.Day > 0)
            {
                if (currentDay.Day > 30)
                {
                    currentDay.Day = 30;
                }
                SaveTotalResult();

                currentData.Days.Add(currentDay);
                currentDay = new DayInfo();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EventManager), nameof(EventManager.ApplyStatus))]
        public static void SaveActionedToDay(CmdType a)
        {
            if (SingletonMonoBehaviour<Settings>.Instance.saveNumber == 0)
                return;
            CommandInfo commandInfo = new CommandInfo();
            commandInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            commandInfo.Command = a;
            currentDay.Commands.Add(commandInfo);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), nameof(EventManager.MidokuMushiTweet))]
        public static void SaveSkippedDM()
        {
            if (currentData == null) return;
            if (currentData.Days == null || currentData.Days.Count == 0)
                return;
            if (currentData.Days[currentData.Days.Count - 1].Commands == null
                || currentData.Days[currentData.Days.Count - 1].Commands.Count == 0)
                return;
            currentData.Days[currentData.Days.Count - 1].Commands[currentData.Days[currentData.Days.Count - 1].Commands.Count - 1].SkippedDM = true;
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(Action_HaishinEnd), nameof(Action_HaishinEnd.startEvent), new Type[] { typeof(CancellationToken) })]
        //[HarmonyPatch(MethodType.Enumerator)]
        public static void SaveStreamToDay()
        {
            CommandInfo commandInfo = new CommandInfo();
            commandInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            commandInfo.SkippedDM = SingletonMonoBehaviour<EventManager>.Instance.isThisTurnMidokuMushi;
            commandInfo.Command = SingletonMonoBehaviour<EventManager>.Instance.executingAction;
            currentDay.Commands.Add(commandInfo);
        }

        public static CommandInfo SaveCommandToDay(CmdType cmdType, bool isSkipDM = false)
        {
            CommandInfo commandInfo = new CommandInfo();
            commandInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            commandInfo.SkippedDM = isSkipDM;
            commandInfo.Command = cmdType;
            return commandInfo;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Status_Stress2_Day), nameof(Status_Stress2_Day.startEvent), new Type[] { typeof(CancellationToken) })]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SaveStressTwoToDay()
        {
            CommandInfo commandInfo = SaveCommandToDay(CmdType.DarknessS2);
            currentDay.Commands.Add(commandInfo);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Event_MV_shuroku), nameof(Event_MV_shuroku.startEvent), new Type[] { typeof(CancellationToken) })]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SaveMusicVideoToDay()
        {
            CommandInfo commandInfo = SaveCommandToDay(CmdType.OdekakeOdaiba);
            currentDay.Commands.Add(commandInfo);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Event_Sea), "<startEvent>b__1_1")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SaveBeachToDay()
        {
            CommandInfo commandInfo = SaveCommandToDay((CmdType)200);
            currentDay.Commands.Add(commandInfo);
        }

        public static void SaveStartingResult()
        {

            ResultInfo resultInfo = GetResult();
            currentDay.startingStats = resultInfo;
        }

        public static void SaveTotalResult()
        {
            ResultInfo resultInfo = GetResult();
            currentDay.endingStats = resultInfo;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EndingManager), nameof(EndingManager.Awake))]
        public static void SaveEndingToDay()
        {
            CommandInfo endingInfo = new CommandInfo();
            endingInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            endingInfo.Ending = SingletonMonoBehaviour<EventManager>.Instance.nowEnding;
            currentDay.Commands.Add(endingInfo);
            SaveDayToData(SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex) + 1);
        }

        public static void SaveEventToDay()
        {
            var name = EventLogger.upcomingEvent?.ToString().Replace("ngov3.", "");
            switch (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart))
            {
                case 0:
                    if (!string.IsNullOrEmpty(currentDay.DayEventName))
                        return;
                    currentDay.DayEventName = name;
                    break;
                default:
                    if (!string.IsNullOrEmpty(currentDay.MidnightEventName))
                        return;
                    currentDay.MidnightEventName = name;
                    break;
            }
        }

        public static ResultInfo GetResult()
        {
            ResultInfo resultInfo = new ResultInfo();
            StatusManager s = SingletonMonoBehaviour<StatusManager>.Instance;
            resultInfo.Followers = s.GetStatus(StatusType.Follower);
            resultInfo.Stress = s.GetStatus(StatusType.Stress);
            resultInfo.Affection = s.GetStatus(StatusType.Love);
            resultInfo.Darkness = s.GetStatus(StatusType.Yami);
            resultInfo.StreamStreak = s.GetStatus(StatusType.RenzokuHaishinCount);
            resultInfo.PreAlertBonus = s.GetStatus(StatusType.SNSzizenKokutiBonus) > 0;
            resultInfo.Communication = s.GetStatus(StatusType.DougaTVShabekuriCountBonus);
            resultInfo.Experience = s.GetStatus(StatusType.OirokeCounter);
            resultInfo.Impact = s.GetStatus(StatusType.OkusuriedCounter);
            resultInfo.GamerGirl = s.GetStatus(StatusType.GameCountBonus);
            resultInfo.Cinephile = s.GetStatus(StatusType.CinePhillCountBonus);
            resultInfo.RabbitHole = s.GetStatus(StatusType.Harumagedo);
            resultInfo.LoveCounter = s.GetStatus(StatusType.MadeLoveCounter);
            resultInfo.IgnoreCounter = SingletonMonoBehaviour<EventManager>.Instance.midokumushi;
            resultInfo.PsycheCounter = SingletonMonoBehaviour<EventManager>.Instance.psycheCount;
            return resultInfo;
        }
    }
}
