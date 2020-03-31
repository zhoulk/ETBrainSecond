/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/29
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using LT.Procedure;

namespace LT.Editor
{
    /// <summary>
    /// 流程服务可视化界面
    /// </summary>
    [CustomEditor(typeof(ProviderProcedure))]
    public class InspectorProviderProcedure : UnityEditor.Editor
    {
        private string[] allProcedureNames;
        private SerializedProperty availableProcedureTypeNames;
        private SerializedProperty entranceProcedureTypeName;
        private List<string> currentAvailableProcedureTypeNames = new List<string>();
        private int entranceProcedureIndex = -1;

        /// <summary>
        /// 显示时
        /// </summary>
        private void OnEnable()
        {
            availableProcedureTypeNames = serializedObject.FindProperty("availableProcedureTypeNames");
            entranceProcedureTypeName = serializedObject.FindProperty("entranceProcedureTypeName");

            LoadAllProcedure();
            ReadAvailableProcedureTypeNames();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 绘图界面时
        /// </summary>
        public override void OnInspectorGUI()
        {
            var provider = (ProviderProcedure)target;
            serializedObject.Update();

            DrawProcedures(provider, allProcedureNames);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 绘制流程
        /// </summary>
        /// <param name="root"></param>
        /// <param name="allProcedureNames"></param>
        private void DrawProcedures(Component root, string[] allProcedureNames)
        {
            GUILayout.Space(5);

            if (EditorApplication.isPlaying)
            {
                InspectorTool.LabelBox("GUI Procedures:", () =>
                {
                    EditorGUILayout.HelpBox("Stop running to modify.", MessageType.Info);
                });
                return;
            }

            InspectorTool.LabelBox("GUI Procedures:", () =>
            {
                if (allProcedureNames.Length <= 0)
                {
                    EditorGUILayout.HelpBox("No optional procedure", MessageType.Info);
                    return;
                }

                InspectorTool.Vertical(() =>
                {
                    foreach (var procedureName in allProcedureNames)
                    {
                        bool selected = currentAvailableProcedureTypeNames.Contains(procedureName);
                        if (selected != EditorGUILayout.ToggleLeft(procedureName, selected))
                        {
                            if (!selected)
                            {
                                currentAvailableProcedureTypeNames.Add(procedureName);
                                WriteAvailableProcedureTypeNames();
                            }
                            else
                            {
                                currentAvailableProcedureTypeNames.Remove(procedureName);
                                WriteAvailableProcedureTypeNames();
                            }
                        }
                    }
                });
            });


            if (currentAvailableProcedureTypeNames.Count > 0)
            {
                EditorGUILayout.Separator();

                int selectedIndex = EditorGUILayout.Popup("Entrance Procedure", entranceProcedureIndex, currentAvailableProcedureTypeNames.ToArray());
                if (selectedIndex != entranceProcedureIndex)
                {
                    entranceProcedureIndex = selectedIndex;
                    entranceProcedureTypeName.stringValue = currentAvailableProcedureTypeNames[selectedIndex];
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select available procedures first.", MessageType.Info);
            }
        }

        /// <summary>
        /// 获取所有流程
        /// </summary>
        private void LoadAllProcedure()
        {
            allProcedureNames = typeof(ProcedureBase).GetSubTypeNames();
        }

        /// <summary>
        /// 读取所有可用流程
        /// </summary>
        private void ReadAvailableProcedureTypeNames()
        {
            currentAvailableProcedureTypeNames = new List<string>();
            int count = availableProcedureTypeNames.arraySize;
            for (int i = 0; i < count; i++)
            {
                currentAvailableProcedureTypeNames.Add(availableProcedureTypeNames.GetArrayElementAtIndex(i).stringValue);
            }

            //删除项目中已经不存在的流程,可能被重构或删除了
            for (int i = 0; i < currentAvailableProcedureTypeNames.Count; i++)
            {
                bool vaild = false;
                foreach (var procedureName in allProcedureNames)
                {
                    if (currentAvailableProcedureTypeNames[i].Equals(procedureName))
                        vaild = true;
                }

                if (!vaild)
                {
                    currentAvailableProcedureTypeNames.RemoveAt(i);
                    i--;
                }
            }

            WriteAvailableProcedureTypeNames();
        }

        /// <summary>
        /// 写入所有可用流程
        /// </summary>
        private void WriteAvailableProcedureTypeNames()
        {
            availableProcedureTypeNames.ClearArray();
            currentAvailableProcedureTypeNames.Sort();

            int count = currentAvailableProcedureTypeNames.Count;
            for (int i = 0; i < count; i++)
            {
                availableProcedureTypeNames.InsertArrayElementAtIndex(i);
                availableProcedureTypeNames.GetArrayElementAtIndex(i).stringValue = currentAvailableProcedureTypeNames[i];
            }

            if (!string.IsNullOrEmpty(entranceProcedureTypeName.stringValue))
            {
                entranceProcedureIndex = currentAvailableProcedureTypeNames.IndexOf(entranceProcedureTypeName.stringValue);
                if (entranceProcedureIndex < 0)
                {
                    entranceProcedureTypeName.stringValue = null;
                }
            }
        }
    }
}