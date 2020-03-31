/*
 *    描述:
 *          1. App 打包
 *
 *    开发人: 邓平
 */
using System;
using System.Collections.Generic;
using System.IO;
using LtFramework.ResKit;
using UnityEngine;
using UnityEditor;

namespace LtFramework.Editor
{
    public class BuildApp
    {
        private static readonly string _AppName = PlayerSettings.productName;
        private static readonly string _ABPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";
        private static readonly string _BuildPath = Application.dataPath + "/../BuildTarget/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";


        [MenuItem("LtUIFrame/App/Build")]
        public static void Build()
        {
            LoadResMode mode = LoadResMode.Resource;
            ResKitConfig config = Resources.Load<ResKitConfig>(typeof(ResKitConfig).Name);
            if (config)
            {
                if (config.LoadResMode == LoadResMode.AssetBundle)
                {
                    mode = LoadResMode.AssetBundle;
                }
            }
            if (mode == LoadResMode.AssetBundle)
            {
                BuildAssetBundle.BuildAB();
                Copy(_ABPath, ResKitConst.AssetBundlePath);
            }


            if (!Directory.Exists(_BuildPath))
            {
                Directory.CreateDirectory(_BuildPath);
            }

            string savePath = "";
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    savePath = _BuildPath + _AppName +
                               string.Format("_{0:yyyy-MM-dd-HH-mm}.apk", System.DateTime.Now);
                    break;
                case BuildTarget.iOS:
                    savePath = _BuildPath + _AppName + string.Format("_{0:yyyy-MM-dd-HH-mm}", System.DateTime.Now);
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    savePath = _BuildPath + _AppName +
                               string.Format("_{0:yyyy-MM-dd-HH-mm}/{1}.exe", System.DateTime.Now, _AppName);
                    break;
                default:
                    savePath = _BuildPath + _AppName + "_" +
                               string.Format("{0:yyyy-MM-dd-HH-mm}", System.DateTime.Now);
                    break;
            }

            BuildPipeline.BuildPlayer(FindEnableEditorScenes(), savePath, EditorUserBuildSettings.activeBuildTarget,
                BuildOptions.None);

            if (mode == LoadResMode.AssetBundle)
            {
                DeleteDir(Application.streamingAssetsPath);
            }

            Debug.Log("执行打包完成 " + savePath);

#if UNITY_EDITOR_WIN
            string path = _BuildPath.Replace("/" , "\\");

            System.Diagnostics.Process.Start("explorer.exe", path);
#endif
        }

        private static string[] FindEnableEditorScenes()
        {
            List<string> editorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                {
                    continue;
                }

                editorScenes.Add(scene.path);
            }

            return editorScenes.ToArray();
        }

        private static void Copy(string srcPath, string targetPath)
        {
            try
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                string scrdir = Path.Combine(targetPath, Path.GetFileName(srcPath));
                if (Directory.Exists(srcPath))
                {
                    scrdir += Path.DirectorySeparatorChar;
                }

                if (!Directory.Exists(scrdir))
                {
                    Directory.CreateDirectory(srcPath);
                }

                string[] files = Directory.GetFileSystemEntries(srcPath);
                foreach (string file in files)
                {
                    if (Directory.Exists(file))
                    {
                        Copy(file, scrdir);
                    }
                    else
                    {
                        File.Copy(file, scrdir + Path.GetFileName(file), true);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError("无法复制:" + srcPath + "到" + targetPath);
            }
        }

        public static void DeleteDir(string scrPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(scrPath);
                FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();
                foreach (FileSystemInfo info in fileInfo)
                {
                    if (info is DirectoryInfo)
                    {
                        DirectoryInfo subDir = new DirectoryInfo(info.FullName);
                        subDir.Delete(true);
                    }
                    else
                    {
                        File.Delete(info.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError("删除失败");
            }
        }
    }
}
