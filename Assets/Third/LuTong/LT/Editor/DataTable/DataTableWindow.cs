/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/18
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.IO;
using UnityEngine;
using UnityEditor;

namespace LT.Editor.DataTable
{
    public class DataTableWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            GetWindowWithRect<DataTableWindow>(new Rect(0, 0, 720, 300)).Show();
        }

        private void OnGUI()
        {
            InspectorTool.Horizontal(() =>
            {
                EditorGUILayout.PrefixLabel("Excel:", EditorStyles.miniBoldLabel);
                DataTableGenerator.DataTablePath = EditorGUILayout.TextField(DataTableGenerator.DataTablePath, EditorStyles.foldout);

                if (InspectorTool.Button("Browse", "", true, 100, 24, EditorStyles.miniButtonRight))
                {
                    string fullPath = UnityEngine.Application.dataPath + DataTableGenerator.DataTablePath;
                    if (!Directory.Exists(fullPath))
                    {
                        fullPath = UnityEngine.Application.dataPath;
                    }
                    DataTableGenerator.DataTablePath = FolderHelper.OpenFilePanel(fullPath);
                }
            });

            InspectorTool.Horizontal(() =>
            {
                EditorGUILayout.PrefixLabel("Output Script:", EditorStyles.miniBoldLabel);
                DataTableGenerator.CSharpCodePath = EditorGUILayout.TextField(DataTableGenerator.CSharpCodePath, EditorStyles.foldout);

                if (InspectorTool.Button("Browse", "", true, 100, 24, EditorStyles.miniButtonRight))
                {
                    string fullPath = UnityEngine.Application.dataPath + DataTableGenerator.CSharpCodePath;
                    if (!Directory.Exists(fullPath))
                    {
                        fullPath = UnityEngine.Application.dataPath;
                    }
                    DataTableGenerator.CSharpCodePath = FolderHelper.OpenFolderPanel(fullPath);
                }
            });

            InspectorTool.Horizontal(() =>
            {
                EditorGUILayout.PrefixLabel("Output Asset:", EditorStyles.miniBoldLabel);
                DataTableGenerator.AssetPath = EditorGUILayout.TextField(DataTableGenerator.AssetPath, EditorStyles.foldout);

                if (InspectorTool.Button("Browse", "", true, 100, 24, EditorStyles.miniButtonRight))
                {
                    string fullPath = UnityEngine.Application.dataPath + DataTableGenerator.AssetPath;
                    if (!Directory.Exists(fullPath))
                    {
                        fullPath = UnityEngine.Application.dataPath;
                    }
                    DataTableGenerator.AssetPath = FolderHelper.OpenFolderPanel(fullPath);
                }
            });

            if (GUILayout.Button("build", EditorStyles.miniButton))
            {
                DataTableGenerator.GenerateDataTables();
                AssetDatabase.Refresh();
            }
        }
    }
}