using GameFramework;
using System;

namespace ETBrain
{
    public static class WebUtility
    {
        public static string EscapeString(string stringToEscape)
        {
            return Uri.EscapeDataString(stringToEscape);
        }

        public static string UnescapeString(string stringToUnescape)
        {
            return Uri.UnescapeDataString(stringToUnescape);
        }

        public static string GetUFUrl(string path)
        {
            return Utility.Text.Format("{0}{1}", GameEntry.Config.GetStringET(Constant.Config.UFUrl), path);
        }

        public static string GetEasyUrl(string path)
        {
            return Utility.Text.Format("{0}{1}", GameEntry.Config.GetStringET(Constant.Config.EasyUrl), path);
        }
    }
}
