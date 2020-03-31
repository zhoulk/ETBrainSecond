/*-------------------------------------------------------------------------------
 * 创建者：#AUTHERNAME#
 * 修改者列表：
 * 创建日期：#CREATEDATE#
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;
using System.IO;

namespace LT.Editor
{
    /// <summary>
    /// 创建文件信息
    /// </summary>
    public class CreateFileInfo : UnityEditor.AssetModificationProcessor
    {
        private const string AuthorName = "huangyechuan";
        private const string DateFormat = "yyyy/MM/dd";

        private static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string allText = File.ReadAllText(path);
                allText = allText.Replace("#AUTHERNAME#", AuthorName);
                allText = allText.Replace("#CREATEDATE#", System.DateTime.Now.ToString(DateFormat));
                File.WriteAllText(path, allText);
                UnityEditor.AssetDatabase.Refresh();
            }
        }
    }
}

