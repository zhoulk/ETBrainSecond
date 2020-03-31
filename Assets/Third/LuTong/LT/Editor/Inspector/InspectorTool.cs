/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/3/21
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace LT.Editor
{
    /// <summary>
    /// 编辑栏工具
    /// </summary>
    public sealed class InspectorTool
    {
        /// <summary>
        /// GUI样式
        /// </summary>
        private static GUIStyle guiStyle;

        /// <summary>
        /// GUI样式
        /// </summary>
        public static GUIStyle GUIStyle
        {
            get { return guiStyle = guiStyle ?? GUI.skin.FindStyle("box"); }
            set { guiStyle = value; }
        }

        /// <summary>
        /// 绘制水平内容
        /// </summary>
        /// <param name="closure"></param>
        public static void Horizontal(Action closure)
        {
            EditorGUILayout.BeginHorizontal(GUIStyle);
            try
            {
                closure();
            }
            finally
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// 绘制垂直内容
        /// </summary>
        /// <param name="closure"></param>
        public static void Vertical(Action closure)
        {
            EditorGUILayout.BeginVertical(GUIStyle);
            try
            {
                closure();
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// 绘制标签盒子
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="codeBlock">代码块</param>
        public static void LabelBox(string title, Action codeBlock)
        {
            Vertical(() =>
            {
                GUILayout.Label(title, EditorStyles.boldLabel);
                Vertical(codeBlock);
            });
        }

        /// <summary>
        /// 绘制折叠盒子
        /// </summary>
        /// <param name="visiable">是否是可见的</param>
        /// <param name="title">标题</param>
        /// <param name="codeBlock">代码块</param>
        /// <returns></returns>
        public static bool ToggleBox(bool visiable, string title, Action codeBlock)
        {
            Vertical(() =>
            {
                visiable = GUILayout.Toggle(visiable, title, EditorStyles.foldout);
                if (!visiable)
                {
                    EditorGUILayout.EndVertical();
                }
                Vertical(codeBlock);
            });
            return visiable;
        }

        /// <summary>
        /// 绘制按钮
        /// </summary>
        /// <param name="title">按钮标题</param>
        /// <param name="tooltip">按钮提示</param>
        /// <param name="enabled">按钮是否是可用的</param>
        /// <param name="width">按钮宽度</param>
        /// <param name="height">按钮高度</param>
        /// <param name="style">样式信息</param>
        /// <returns></returns>
        public static bool Button(string title, string tooltip, bool enabled, float width = -1, float height = -1, GUIStyle style = null)
        {
            var widthOptions = (width <= 0) ? GUILayout.ExpandWidth(true) : GUILayout.Width(width);
            var heightOptions = (height <= 0) ? GUILayout.ExpandHeight(true) : GUILayout.Height(height);
            style = style ?? EditorStyles.miniButton;
            if (enabled)
            {
                return GUILayout.Button(new GUIContent(title, tooltip), style, widthOptions, heightOptions);
            }

            return (bool)ApplyColor(
                () => GUILayout.Button(new GUIContent(title, tooltip), style, widthOptions,
                    heightOptions), new Color(1f, 1f, 1f, 0.25f));
        }

        /// <summary>
        /// 应用颜色
        /// </summary>
        /// <param name="action">闭包</param>
        /// <param name="color">指定颜色</param>
        public static object ApplyColor(Func<object> action, Color color)
        {
            return ApplyColor(action, color, GUI.contentColor);
        }

        /// <summary>
        /// 应用颜色
        /// </summary>
        /// <param name="action">闭包</param>
        /// <param name="color">指定颜色</param>
        /// <param name="contentColor">文字颜色</param>
        public static object ApplyColor(Func<object> action, Color color, Color contentColor)
        {
            var backup = GUI.color;
            var backupContent = GUI.contentColor;
            try
            {
                GUI.color = color;
                GUI.contentColor = contentColor;
                return action();
            }
            finally
            {
                GUI.color = backup;
                GUI.contentColor = backupContent;
            }
        }

        /// <summary>
        /// 测试是否处于需要检查的程序集列表
        /// </summary>
        /// <param name="assembly">测试程序集</param>
        /// <returns>是否处于忽略程序集列表</returns>
        public static bool TestCheckInAssembiles(Assembly assembly)
        {
            foreach (var pattern in ignoreAssembiles)
            {
                if (Str.Is(pattern, assembly.GetName().Name))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 需要忽略的程序集
        /// </summary>
        private static readonly string[] ignoreAssembiles = new string[]
        {
            "mscorlib",
            "UnityEngine",
            "UnityEditor",
            "netstandard",
            "nunit.framework",
            "System",
            "ExCSS.Unity",
            "UnityEditor.*",
            "UnityEngine.*",
            "*-Editor",
            "*.Editor",
            "System.*",
            "Mono.*",
            "Unity.*",
            "SyntaxTree.*",
            "Microsoft.*",
        };
    }
}