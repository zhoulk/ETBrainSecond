/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/3/25
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEditor;
using System.IO;
using UnityEngine;
using LT.Setting;

namespace LT.Editor
{
    public partial class MenuItems
    {
        [MenuItem("Tools/LT/清理 Setting", false, 0)]
        public static void ClearSetting()
        {
            PlayerPrefs.DeleteAll();

            if (File.Exists(SettingJson.SavedPath))
            {
                File.Delete(SettingJson.SavedPath);
            }

            Debug.Log("清理 Setting 完成.");
        }

        [MenuItem("Tools//LT/启动 Web Server", false, int.MaxValue)]
        public static void OpenFileServer()
        {
            ProcessHelper.Run("dotnet", "FileServer.dll", "../FileServer/");
        }
    }
}