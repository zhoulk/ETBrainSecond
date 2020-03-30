﻿using GameFramework;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public static class ConfigExtension
    {
        public static void LoadConfig(this ConfigComponent configComponent, string configName, LoadType loadType, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("Config name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName, AssetUtility.GetConfigAsset(configName, loadType), loadType, Constant.AssetPriority.ConfigAsset, userData);
        }

        public static string GetStringET(this ConfigComponent configComponent, string configName)
        {
            if (BuiltinDataComponent.Enviroment.Develop.Equals(GameEntry.Builtin.Env))
            {
                configName = Utility.Text.Format("Dev.{0}", configName);
            }
            return configComponent.GetString(configName);
        }
    }
}
