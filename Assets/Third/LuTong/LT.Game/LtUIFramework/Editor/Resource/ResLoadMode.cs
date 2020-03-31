/*
 *    描述:
 *          1.资源加载模式
 *
 *    开发人: 邓平
 */

using System.IO;
using LtFramework.ResKit;
using UnityEditor;
using UnityEngine;

namespace LtFramework.Editor
{
    public class ResLoadMode
    {
        private const string _EditorLoadModePath = "LtUIFrame/ResKit/编辑器模式加载";
        private const string _LoadResModePath = "LtUIFrame/ResKit/AssetBundle加载";

        private const string _ResKitConfigPath = "Assets/Resources/";

        private static void CreateResAsset(bool isLoadAssetBundle)
        {
#if UNITY_EDITOR

            if (!Directory.Exists(_ResKitConfigPath))
            {
                Directory.CreateDirectory(_ResKitConfigPath);
            }

            string fileName = _ResKitConfigPath + typeof(ResKitConfig).Name + ".asset";

            if (File.Exists(fileName))
            {
                ResKitConfig temp = Resources.Load<ResKitConfig>(typeof(ResKitConfig).Name);
                if (temp)
                {
                    temp.LoadResMode = isLoadAssetBundle ? ResKit.LoadResMode.AssetBundle : ResKit.LoadResMode.Resource;
                    return;
                }
                else
                {
                    File.Delete(fileName);
                }
            }

            ResKitConfig config = ScriptableObject.CreateInstance<ResKitConfig>();
            config.LoadResMode = isLoadAssetBundle ? ResKit.LoadResMode.AssetBundle : ResKit.LoadResMode.Resource;

            UnityEditor.AssetDatabase.CreateAsset(config, fileName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        private static bool GetResAsset()
        {
            if (!Directory.Exists(_ResKitConfigPath))
            {
                return false;
            }

            string fileName = _ResKitConfigPath + typeof(ResKitConfig).Name + ".asset";

            if (!File.Exists(fileName))
            {
                return false;
            }


            ResKitConfig config = Resources.Load<ResKitConfig>(typeof(ResKitConfig).Name);
            if (config)
            {
                return (int) config.LoadResMode == 1;
            }

            return false;
        }

        private static bool LoadResMode
        {
            get => GetResAsset();
            set => CreateResAsset(value);
        }


        [MenuItem(_LoadResModePath)]
        private static void ToggleLoadResMode()
        {
            Debug.Log(111111111);
            LoadResMode = !LoadResMode;
        }

        [MenuItem(_LoadResModePath, true)]
        public static bool ToggleLoadResValidate()
        {
            Menu.SetChecked(_LoadResModePath, LoadResMode);
            //ResKitConfig config = Resources.Load<ResKitConfig>(typeof(ResKitConfig).Name);
            //if (config)
            //{
            //    return false;
            //}
            return true;
        }


        private static bool EditorLoadMode
        {
            get => ResManager.EditorLoadMode;
            set => ResManager.EditorLoadMode = value;
        }

        [MenuItem(_EditorLoadModePath)]
        private static void ToggleEditorLoadMode()
        {
            EditorLoadMode = !EditorLoadMode;
        }

        [MenuItem(_EditorLoadModePath, true)]
        public static bool ToggleEditorLoadModeValidate()
        {
            Menu.SetChecked(_EditorLoadModePath, EditorLoadMode);
            return true;
        }

    }
}
