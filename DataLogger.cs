using HarmonyLib;
using ngov3;
using System;
using System.Threading;

namespace PlaythroughLogs
{
    [HarmonyPatch]
    internal class DataLogger
    {
        internal static PlaythroughLog currentLog = new PlaythroughLog();

        internal static DataInfo currentData = new DataInfo();
        internal static DayInfo currentDay = new DayInfo();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), "Load")]
        [HarmonyPatch(typeof(EventManager), "StartOver")]
        public static void SetDataInfo()
        {
            int saveNum = SingletonMonoBehaviour<Settings>.Instance.saveNumber;
            if (currentData.SaveNum != saveNum)
            {
                if (currentData.Days.Count > 0)
                {
                    currentLog.Datas.Add(currentData);
                }
                SetDataInfo(ref currentData, saveNum);
            }

        }

        public static void SetDataInfo(ref DataInfo info, int saveNum)
        {
            info = new DataInfo();
            info.SaveNum = saveNum;
            SaveStartingResult();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), "Save")]
        public static void SaveDayToData()
        {
            SaveTotalResult();
            currentData.Days.Add(currentDay);
            currentDay = new DayInfo();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EventManager), nameof(EventManager.ExecuteActionConfirmed))]
        public static void SaveCommandToDay(EventManager __instance, CmdType a, bool isEventCommand)
        {
            CommandInfo commandInfo = new CommandInfo();
            commandInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            commandInfo.Command = a;
            commandInfo.SkippedDM = !__instance.checkTuutiCleaned() && !isEventCommand;
            currentDay.Commands.Add(commandInfo);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Action_HaishinEnd), nameof(Action_HaishinEnd.startEvent), new Type[] { typeof(CancellationToken) })]
        public static CommandInfo SaveStreamToDay(EventManager __instance)
        {
            CommandInfo commandInfo = new CommandInfo();
            commandInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            commandInfo.SkippedDM = SingletonMonoBehaviour<EventManager>.Instance.isThisTurnMidokuMushi;
            commandInfo.Command = SingletonMonoBehaviour<EventManager>.Instance.executingAction;
            return commandInfo;
        }

        public static CommandInfo SaveCommandToDay(CmdType cmdType, bool isSkipDM = false)
        {
            CommandInfo commandInfo = new CommandInfo();
            commandInfo.DayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            commandInfo.SkippedDM = isSkipDM;
            commandInfo.Command = cmdType;
            return commandInfo;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Status_Stress2_Day), nameof(Status_Stress2_Day.startEvent), new Type[] { typeof(CancellationToken) })]
        public static void SaveStressTwoToDay()
        {
            CommandInfo commandInfo = SaveCommandToDay(CmdType.DarknessS2);
            currentDay.Commands.Add(commandInfo);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Event_MV_shuroku), nameof(Event_MV_shuroku.startEvent), new Type[] { typeof(CancellationToken) })]
        public static void SaveMusicVideoToDay()
        {
            CommandInfo commandInfo = SaveCommandToDay(CmdType.OdekakeOdaiba);
            currentDay.Commands.Add(commandInfo);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Event_Sea), "<startEvent>b__1_1")]
        [HarmonyPatch(MethodType.Enumerator)]
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
            SaveDayToData();
        }

        public static void SaveEventToDay()
        {
            switch (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart))
            {
                case 0:
                    currentDay.DayEventName = EventLogger.upcomingEvent.ToString();
                    break;
                case 3:
                    currentDay.MidnightEventName = EventLogger.upcomingEvent.ToString();
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
