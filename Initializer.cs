﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


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
            ExportLogs.SetPlaythroughId();
            ExportLogs.CreateLogDirectory();
            ExportLogs.CreateCurrentLogDirectory();
            ExportLogs.SetLangSettings();

        }


        public void OnApplicationQuit()
        {
            DataLogger.AddDataToCurrentLog();
            ExportLogs.SavePlaythroughToJSON();
            ExportLogs.SaveDataLogsToCSV();
        }
    }

}


