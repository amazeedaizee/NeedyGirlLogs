using HarmonyLib;
using ngov3;
using System;

namespace PlaythroughLogs
{
    [HarmonyPatch]
    internal class EventLogger
    {
        internal static bool statusApplied;
        internal static NgoEvent upcomingEvent;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EventManager), "StartEvent")]
        internal static void EventPeeker(EventManager __instance)
        {
            if (__instance.eventQueue.Count > 0)
            {
                NgoEvent ngoEvent = __instance.eventQueue.Peek();

                if (ngoEvent.ToString().Contains("Action_")) { return; }
                if (ngoEvent.ToString().Contains("Ending_")) { return; }
                if (ngoEvent.ToString().Contains("DayPassing")) { return; }
                if (ngoEvent.ToString().Contains("Separator")) { return; }
                if (ngoEvent.ToString().Contains("TimePassing")) { return; }
                if (ngoEvent.ToString().Contains("_Uzagarami")) { return; }
                if (ngoEvent.ToString().Contains("_CheckBGM")) { return; }
                upcomingEvent = ngoEvent;
                DataLogger.SaveEventToDay();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Boot), "Awake")]
        [HarmonyPatch(typeof(EventManager), "EndEvent", new Type[] { })]
        internal static void ClearPeekedEvent()
        {
            if (upcomingEvent != null)
            {
                upcomingEvent = null;
            }
        }
    }
}
