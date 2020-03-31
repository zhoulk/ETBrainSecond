/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/3/19
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LT.Editor
{
    /// <summary>
    /// 框架入口可视化图形界面
    /// </summary>
    [CustomEditor(typeof(Framework), true)]
    public class InspectorFramework : UnityEditor.Editor
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        private SerializedProperty debugLevel;

        /// <summary>
        /// 已经安装了的服务提供者列表
        /// </summary>
        private Dictionary<Type, Component> serviceProviders = new Dictionary<Type, Component>();

        /// <summary>
        /// 绘图界面时
        /// </summary>
        public override void OnInspectorGUI()
        {
            var framework = (Framework)target;
            framework.name = "App";

            serializedObject.Update();
            DrawTitle();
            DrawDebugLevels(framework);
            DrawServiceProvider(framework, serviceProviders);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 显示时
        /// </summary>
        public void OnEnable()
        {
            debugLevel = serializedObject.FindProperty("DebugLevel");

            RefreshServiceProviders();
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        private static void DrawTitle()
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Framework (" + App.Version + ")", EditorStyles.largeLabel, GUILayout.Height(20));
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制调试等级
        /// </summary>
        private void DrawDebugLevels(Framework framework)
        {
            var old = debugLevel.enumValueIndex;
            GUILayout.BeginHorizontal();
            debugLevel.enumValueIndex = (int)(DebugLevel)EditorGUILayout.EnumPopup("Debug Level", (DebugLevel)debugLevel.enumValueIndex, EditorStyles.popup);
            GUILayout.EndHorizontal();

            if (old == debugLevel.enumValueIndex)
            {
                return;
            }

            if (framework.Application != null)
            {
                framework.Application.DebugLevel = (DebugLevel)debugLevel.enumValueIndex;
            }
        }

        /// <summary>
        /// 绘制服务提供者
        /// </summary>
        /// <param name="root">根节点信息</param>
        /// <param name="serviceProviders">服务提供者列表</param>
        private void DrawServiceProvider(Component root, IDictionary<Type, Component> serviceProviders)
        {
            GUILayout.Space(5);

            if (EditorApplication.isPlaying)
            {
                InspectorTool.LabelBox("GUI Service Providers:", () =>
                {
                    EditorGUILayout.HelpBox("Stop running to modify.", MessageType.Info);
                });
                return;
            }

            var reload = false;
            InspectorTool.LabelBox("GUI Service Providers:", () =>
            {
                if (serviceProviders.Count <= 0)
                {
                    EditorGUILayout.HelpBox("No optional service provider", MessageType.Info);
                    return;
                }

                foreach (var providerAndGameObject in serviceProviders)
                {
                    var enable = providerAndGameObject.Value != null;
                    InspectorTool.Horizontal(() =>
                    {
                        reload = ToggleProvider(root.gameObject, providerAndGameObject.Key, providerAndGameObject.Value) != enable || reload;

                        if (InspectorTool.Button("Go", "Goto The GameObject", enable, 40, 16) && enable)
                        {
                            Selection.activeObject = providerAndGameObject.Value;
                        }
                    });
                }
            });

            if (reload)
            {
                RefreshServiceProviders();
            }
        }

        /// <summary>
        /// 服务提供者开关
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="providerType">服务提供者类型</param>
        /// <param name="master">宿主对象</param>
        private bool ToggleProvider(GameObject root, Type providerType, Component master)
        {
            if (EditorGUILayout.ToggleLeft(providerType.Name, master != null))
            {
                if (master == null)
                {
                    CreateServiceProvider(providerType, root);
                }
                return true;
            }

            if (master == null)
            {
                return false;
            }

            if (root == master.gameObject)
            {
                EditorUtility.DisplayDialog("Failure", providerType.Name + " is located in the main game object, please manually delete the script", "Ok");
                return true;
            }

            if (!EditorUtility.DisplayDialog("Deleting", "The " + providerType.Name + " will be Deleted" + Environment.NewLine + "Configuration will be lost", "Delete", "Cancel"))
            {
                return true;
            }

            DestroyImmediate(master.gameObject);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            return false;
        }

        /// <summary>
        /// 创建服务提供者
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="providerType">服务提供者类型</param>
        private static void CreateServiceProvider(Type providerType, GameObject root)
        {
            var go = new GameObject(providerType.Name);
            go.AddComponent(providerType);
            GameObjectUtility.SetParentAndAlign(go, root);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        /// <summary>
        /// 刷新服务提供者信息
        /// </summary>
        private void RefreshServiceProviders()
        {
            serviceProviders.Clear();
            LoadAllServiceProviders(serviceProviders);
            LoadInstalledServiceProviders(serviceProviders, (Component)target);

            //排序
            serviceProviders = serviceProviders.OrderBy(k => k.Key.Name).ToDictionary(v => v.Key, v => v.Value);
        }

        /// <summary>
        /// 获取已经安装了的服务提供者
        /// </summary>
        /// <param name="result">结果集</param>
        /// <param name="target">扫描的根组件</param>
        /// <returns>安装关系字典</returns>
        private static void LoadInstalledServiceProviders(IDictionary<Type, Component> result, Component target)
        {
            foreach (var serviceProvider in target.gameObject.GetComponentsInChildren<IServiceProvider>())
            {
                result[serviceProvider.GetType()] = (Component)serviceProvider;
            }
        }

        /// <summary>
        /// 获取GUI支持的服务提供者
        /// </summary>
        /// <returns>编辑器框架</returns>
        private static void LoadAllServiceProviders(IDictionary<Type, Component> result)
        {
            var targetProvider = typeof(IServiceProvider);
            var targetComponent = typeof(Component);
            var assembiles = Arr.Filter(AppDomain.CurrentDomain.GetAssemblies(), InspectorTool.TestCheckInAssembiles);

            foreach (var assembly in assembiles)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (targetProvider.IsAssignableFrom(type) && targetComponent.IsAssignableFrom(type))
                    {
                        result.Add(type, null);
                    }
                }
            }
        }
    }
}