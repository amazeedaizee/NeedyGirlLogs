using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace PlaythroughLogs
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("Windose.exe")]
    public class NGOPlugin : BaseUnityPlugin
    {
        public const string pluginGuid = "needy.girl.logs";
        public const string pluginName = "Logs";
        public const string pluginVersion = "1.0.0.0";

        public static ManualLogSource logger;
        public static PluginInfo PInfo { get; private set; }
        public void Awake()
        {
            PInfo = Info;

            logger = Logger;

            Logger.LogInfo("Saves actions and events in a day in the form of logs. Logs appear after the game is closed.");

            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
        }

        public void Start()
        {
            ExportLogs.InitializeExporter();

        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.ScrollLock))
            {
                ExportLogs.InstantExport();
                ExportLogs.InitializeExporter();
                DataLogger.currentLog.DataOnes.Clear();
                DataLogger.currentLog.DataTwos.Clear();
                DataLogger.currentLog.DataThrees.Clear();
            }
        }

        public void OnApplicationQuit()
        {
            ExportLogs.InstantExport();
        }
    }

}


