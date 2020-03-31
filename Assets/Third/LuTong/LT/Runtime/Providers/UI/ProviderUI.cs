/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using UnityEngine;
using LT.ObjectPool;
using LT.Container;

namespace LT.UI
{
    /// <summary>
    /// UI管理服务
    /// </summary>
    public class ProviderUI : MonoBehaviour, IServiceProvider
    {
        [Header("UI根节点")]
        public Transform UIGroupRoot;

        [Header("UI组配置")]
        public UIGroupConfig[] UIGroups;

        /// <inheritdoc />
        public void Register()
        {
            App.SingletonIf<IObjectPoolManager, ObjectPoolManager>();
            App.Singleton<IUIManager, UIManager>();
        }

        /// <inheritdoc />
        public void Init()
        {
            Guard.Verify<ArgumentException>(UIGroupRoot == null, "UIForm root is invaild.");

            // 获取UI管理器
            IUIManager ui = App.Make<IUIManager>();

            // 设置UIForm辅助器
            ui.SetUIFormHelper(new DefaultUIFormHelper(UIGroupRoot));

            // 初始化分组信息
            foreach (var group in UIGroups)
            {
                if (!ui.HasUIGroup(group.Name))
                {
                    ui.AddUIGroup(group.Name, group.Depth);
                }
            }
        }

        /// <summary>
        /// UIGroup配置
        /// </summary>
        [Serializable]
        public class UIGroupConfig
        {
            /// <summary>
            /// 组名
            /// </summary>
            [SerializeField]
            public string Name;

            /// <summary>
            /// 深度
            /// </summary>
            [SerializeField]
            public int Depth;
        }
    }
}