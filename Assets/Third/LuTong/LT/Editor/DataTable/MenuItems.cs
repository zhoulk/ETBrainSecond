/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/09/28
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEditor;
using LT.Editor.DataTable;
using LT.DataTable;

namespace LT.Editor
{
    public partial class MenuItems
    {
        [MenuItem("Tools/LT/构建 DataTable Code", false, 100)]
        public static void GenerateDataTableCode()
        {
            DataTableGenerator.GenerateDataTables();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/LT/配置 DataTable Code", false, 101)]
        public static void GenerateDataTableCodeConfig()
        {
            DataTableWindow.ShowWindow();
        }

        //[MenuItem("Tools/LT/test", false, 102)]
        //public static void Test()
        //{
        //    var scriptName = "DTBullet";

        //    var instance = ScriptableObject.CreateInstance(scriptName) as ScriptableObjectBase;
        //    AssetDatabase.CreateAsset(instance, "Assets/x.asset");
        //}
    }
}