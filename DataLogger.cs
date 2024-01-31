using HarmonyLib;
using ngov3;

namespace PlaythroughLogs
{
    [HarmonyPatch]
    internal class DataLogger
    {
        internal static DataInfo dataOne = null;
        internal static DataInfo dataTwo = null;
        internal static DataInfo dataThree = null;

        internal static DayInfo currentDay = new DayInfo();

        public static void SetDataInfo()
        {
            switch (SingletonMonoBehaviour<Settings>.Instance.saveNumber)
            {
                case 1:
                    SetDataInfo(dataOne);
                    break;
                case 2:
                    SetDataInfo(dataTwo);
                    break;
                case 3:
                    SetDataInfo(dataThree);
                    break;
            }
        }

        public static void SetDataInfo(DataInfo info)
        {
            DayInfo newDay = new DayInfo();
            if (info == null)
            {
                info = new DataInfo();
            }

        }

        public static void SaveDayToData()
        {
            switch (SingletonMonoBehaviour<Settings>.Instance.saveNumber)
            {
                case 1:
                    dataOne.Days.Add(currentDay);
                    break;
                case 2:
                    dataTwo.Days.Add(currentDay);
                    break;
                case 3:
                    dataThree.Days.Add(currentDay);
                    break;
            }
            currentDay = new DayInfo();
        }

    }
}
