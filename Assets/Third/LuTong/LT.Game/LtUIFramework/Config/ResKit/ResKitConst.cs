/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using UnityEditor;
using UnityEngine;

namespace LtFramework.ResKit
{
    public class ResKitConst
    {
        public const string ResUrl = "res://";
        public const string AbUrl = "ab://";

        //发表app AssetBundle资源路径
        public static readonly string AssetBundlePath = Application.streamingAssetsPath + "/" + Platform;

        public static string Platform
        {
            get
            {
                string platform = string.Empty;
#if UNITY_EDITOR
                switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.StandaloneWindows:
                    case BuildTarget.StandaloneWindows64:
                        platform = "Windows";
                        break;
                    case BuildTarget.Android:
                        platform = "Android";
                        break;
                    case BuildTarget.StandaloneLinux:
                    case BuildTarget.StandaloneLinux64:
                    case BuildTarget.StandaloneLinuxUniversal:
                        platform = "Linux";
                        break;
                    case BuildTarget.StandaloneOSX:
                        platform = "OSX";
                        break;
                    case BuildTarget.iOS:
                        platform = "IOS";
                        break;
                }

                return platform;
#else
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        platform = "Windows";
                        break;
                    case RuntimePlatform.Android:
                        platform = "Android";
                        break;
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.LinuxPlayer:
                        platform = "Linux";
                        break;
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        platform = "OSX";
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        platform = "IOS";
                        break;
                }

                return platform;
#endif

            }
        }
    }
}
