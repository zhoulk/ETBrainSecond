/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using UnityEngine;

namespace LT.UI
{
    /// <summary>
    /// 窗体基类。
    /// </summary>
    public abstract class UIForm : MonoBehaviour, IUIForm
    {
        private string m_UIFormAssetName;
        private IUIGroup m_UIGroup;
        private int m_DepthInUIGroup;
        private bool m_PauseCoveredUIForm;

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        public string UIFormAssetName => m_UIFormAssetName;

        /// <summary>
        /// 获取界面实例。
        /// </summary>
        public object UIFormInstance => gameObject;

        /// <summary>
        /// 获取界面所属的界面组。
        /// </summary>
        public IUIGroup UIGroup => m_UIGroup;

        /// <summary>
        /// 获取界面深度。
        /// </summary>
        public int DepthInUIGroup => m_DepthInUIGroup;

        /// <summary>
        /// 获取是否暂停被覆盖的界面。
        /// </summary>
        public bool PauseCoveredUIForm => m_PauseCoveredUIForm;

        /// <summary>
        /// 可交互的
        /// </summary>
        public virtual bool Interactable => enabled;

        /// <summary>
        /// 初始化界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroup">界面所处的界面组。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnInit(string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm, bool isNewInstance, object userData)
        {
            this.m_UIFormAssetName = uiFormAssetName;

            if (isNewInstance)
            {
                this.m_UIGroup = uiGroup;
            }
            else if (this.m_UIGroup != uiGroup)
            {
                LTLog.Error("UI group is inconsistent for new-instance UI form.");
                return;
            }

            m_DepthInUIGroup = uiGroup.Depth;
            m_PauseCoveredUIForm = pauseCoveredUIForm;

            if (!isNewInstance)
            {
                return;
            }

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnOpen(object userData)
        {
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnClose(object userData)
        {
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        public virtual void OnPause()
        {
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        public virtual void OnResume()
        {
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        public virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        public virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询。
        /// </summary>
        public virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        public virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            this.m_DepthInUIGroup = depthInUIGroup;
        }
    }
}