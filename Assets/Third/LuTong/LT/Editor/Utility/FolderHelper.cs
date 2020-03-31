/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/3/21
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using LT;

namespace LT.Editor
{
    /// <summary>
    /// 文件夹助手。
    /// </summary>
    public static class FolderHelper
    {
        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="currentPath">当前路径</param>
        /// <returns>新路径</returns>
        public static string OpenFolderPanel(string currentPath)
        {
            var newPath = EditorUtility.OpenFolderPanel("OpenFolderPanel", currentPath, string.Empty);

            if (!string.IsNullOrEmpty(newPath))
            {
                var gamePath = System.IO.Path.GetFullPath(".");
                gamePath = gamePath.Replace("\\", "/");
                gamePath += "/Assets";

                if (newPath.StartsWith(gamePath) && newPath.Length > gamePath.Length)
                {
                    newPath = newPath.Remove(0, gamePath.Length);
                }
                currentPath = newPath;
            }

            return currentPath;
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        public static string OpenFilePanel(string currentPath)
        {
            var newPath = EditorUtility.OpenFilePanel("OpenFilePanel", currentPath, string.Empty);

            if (!string.IsNullOrEmpty(newPath))
            {
                var gamePath = System.IO.Path.GetFullPath(".");
                gamePath = gamePath.Replace("\\", "/");
                gamePath += "/Assets";

                if (newPath.StartsWith(gamePath) && newPath.Length > gamePath.Length)
                {
                    newPath = newPath.Remove(0, gamePath.Length);
                }
                currentPath = newPath;
            }

            return currentPath;
        }
    }
}