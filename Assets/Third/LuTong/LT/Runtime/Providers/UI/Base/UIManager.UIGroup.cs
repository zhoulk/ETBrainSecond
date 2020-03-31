/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/01
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace LT.UI
{
    internal sealed partial class UIManager
    {
        /// <summary>
        /// 界面组。
        /// </summary>
        private sealed partial class UIGroup : IUIGroup
        {
            private readonly string m_Name;
            private int m_Depth;
            private bool m_Pause;
            private readonly IUIGroupHelper m_UIGroupHelper;
            private readonly LinkedList<UIFormInfo> m_UIFormInfos;

            /// <summary>
            /// 初始化界面组的新实例。
            /// </summary>
            /// <param name="name">界面组名称。</param>
            /// <param name="depth">界面组深度。</param>
            /// <param name="uiGroupHelper">界面组辅助器。</param>
            public UIGroup(string name, int depth, IUIGroupHelper uiGroupHelper)
            {
                Guard.NotEmptyOrNull(name, "UI group name is invalid.");
                Guard.Requires<ArgumentException>(uiGroupHelper != null, "UI group helper is invalid.");

                this.m_Name = name;
                this.m_Pause = false;
                this.m_UIGroupHelper = uiGroupHelper;
                this.m_UIFormInfos = new LinkedList<UIFormInfo>();
                this.Depth = depth;
            }

            /// <summary>
            /// 获取界面组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取或设置界面组深度。
            /// </summary>
            public int Depth
            {
                get
                {
                    return m_Depth;
                }
                set
                {
                    if (m_Depth == value)
                    {
                        return;
                    }

                    m_Depth = value;
                    m_UIGroupHelper.SetDepth(m_Depth);
                    Refresh();
                }
            }

            /// <summary>
            /// 获取或设置界面组是否暂停。
            /// </summary>
            public bool Pause
            {
                get
                {
                    return m_Pause;
                }
                set
                {
                    if (m_Pause == value)
                    {
                        return;
                    }

                    m_Pause = value;
                    Refresh();
                }
            }

            /// <summary>
            /// 获取界面组中界面数量。
            /// </summary>
            public int UIFormCount
            {
                get
                {
                    return m_UIFormInfos.Count;
                }
            }

            /// <summary>
            /// 获取当前界面。
            /// </summary>
            public IUIForm CurrentUIForm
            {
                get
                {
                    return m_UIFormInfos.First != null ? m_UIFormInfos.First.Value.UIForm : null;
                }
            }

            /// <summary>
            /// 获取界面组辅助器。
            /// </summary>
            public IUIGroupHelper Helper
            {
                get
                {
                    return m_UIGroupHelper;
                }
            }

            /// <summary>
            /// 界面组轮询。
            /// </summary>
            public void Update()
            {
                LinkedListNode<UIFormInfo> current = m_UIFormInfos.First;
                while (current != null)
                {
                    if (current.Value.Paused)
                    {
                        break;
                    }

                    LinkedListNode<UIFormInfo> next = current.Next;

                    if (current.Value.UIForm.Interactable)
                    {
                        current.Value.UIForm.OnUpdate();
                    }

                    current = next;
                }
            }

            /// <summary>
            /// 界面组中是否存在界面。
            /// </summary>
            /// <param name="uiFormAssetName">界面资源名称。</param>
            /// <returns>界面组中是否存在界面。</returns>
            public bool HasUIForm(string uiFormAssetName)
            {
                Guard.NotEmptyOrNull(uiFormAssetName, "UI form asset name is invalid.");

                foreach (UIFormInfo uiFormInfo in m_UIFormInfos)
                {
                    if (uiFormInfo.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 从界面组中获取界面。
            /// </summary>
            /// <param name="uiFormAssetName">界面资源名称。</param>
            /// <returns>要获取的界面。</returns>
            public IUIForm GetUIForm(string uiFormAssetName)
            {
                Guard.NotEmptyOrNull(uiFormAssetName, "UI form asset name is invalid.");

                foreach (UIFormInfo uiFormInfo in m_UIFormInfos)
                {
                    if (uiFormInfo.UIForm.UIFormAssetName == uiFormAssetName)
                    {
                        return uiFormInfo.UIForm;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从界面组中获取所有界面。
            /// </summary>
            /// <returns>界面组中的所有界面。</returns>
            public IUIForm[] GetAllUIForms()
            {
                List<IUIForm> results = new List<IUIForm>();
                foreach (UIFormInfo uiFormInfo in m_UIFormInfos)
                {
                    results.Add(uiFormInfo.UIForm);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 往界面组增加界面。
            /// </summary>
            /// <param name="uiForm">要增加的界面。</param>
            public void AddUIForm(IUIForm uiForm)
            {
                UIFormInfo uiFormInfo = new UIFormInfo(uiForm);
                m_UIFormInfos.AddFirst(uiFormInfo);
            }

            /// <summary>
            /// 从界面组移除界面。
            /// </summary>
            /// <param name="uiForm">要移除的界面。</param>
            public void RemoveUIForm(IUIForm uiForm)
            {
                UIFormInfo uiFormInfo = GetUIFormInfo(uiForm);

                Guard.Requires<ArgumentException>(uiForm != null, $"Can not find UI form asset name is '{uiForm.UIFormAssetName}'.");

                if (!uiFormInfo.Covered)
                {
                    uiFormInfo.Covered = true;
                    uiForm.OnCover();
                }

                if (!uiFormInfo.Paused)
                {
                    uiFormInfo.Paused = true;
                    uiForm.OnPause();
                }

                m_UIFormInfos.Remove(uiFormInfo);
            }

            /// <summary>
            /// 激活界面。
            /// </summary>
            /// <param name="uiForm">要激活的界面。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void RefocusUIForm(IUIForm uiForm, object userData)
            {
                UIFormInfo uiFormInfo = GetUIFormInfo(uiForm);

                Guard.Requires<ArgumentException>(uiFormInfo != null, "Can not find UI form info.");

                m_UIFormInfos.Remove(uiFormInfo);
                m_UIFormInfos.AddFirst(uiFormInfo);
            }

            /// <summary>
            /// 刷新界面组。
            /// </summary>
            public void Refresh()
            {
                var current = m_UIFormInfos.First;
                bool pause = m_Pause;
                bool cover = false;
                int depth = UIFormCount;

                while (current != null)
                {
                    RefreshUIForm(current.Value, pause, cover, depth--);

                    if (!cover)
                    {
                        pause = current.Value.UIForm.PauseCoveredUIForm;
                        cover = true;
                    }

                    current = current.Next;
                }
            }

            /// <summary>
            /// 刷新旧的界面
            /// </summary>
            public void RefreshOnlyOlds()
            {
                var current = m_UIFormInfos.First;
                bool pause = m_Pause ? m_Pause : current.Value.UIForm.PauseCoveredUIForm;
                int depth = UIFormCount - 1;

                current = current.Next;
                while (current != null)
                {
                    RefreshUIForm(current.Value, pause, true, depth--);

                    current = current.Next;
                }
            }

            /// <summary>
            /// 刷新最新的界面
            /// </summary>
            public void RefreshOnlyNew()
            {
                var current = m_UIFormInfos.First;
                int depth = UIFormCount;
                RefreshUIForm(current.Value, m_Pause, false, depth);
            }

            private void RefreshUIForm(UIFormInfo uiFormInfo, bool pause, bool cover, int depthInUIGroup)
            {
                uiFormInfo.UIForm.OnDepthChanged(Depth, depthInUIGroup);

                if (pause)
                {
                    if (!uiFormInfo.Covered)
                    {
                        uiFormInfo.Covered = true;
                        uiFormInfo.UIForm.OnCover();
                    }

                    if (!uiFormInfo.Paused)
                    {
                        uiFormInfo.Paused = true;
                        uiFormInfo.UIForm.OnPause();
                    }
                }
                else
                {
                    if (cover)
                    {
                        if (!uiFormInfo.Covered)
                        {
                            uiFormInfo.Covered = true;
                            uiFormInfo.UIForm.OnCover();
                        }
                    }
                    else
                    {
                        if (uiFormInfo.Paused)
                        {
                            uiFormInfo.Paused = false;
                            uiFormInfo.UIForm.OnResume();
                        }

                        if (uiFormInfo.Covered)
                        {
                            uiFormInfo.Covered = false;
                            uiFormInfo.UIForm.OnReveal();
                        }
                    }
                }
            }

            private UIFormInfo GetUIFormInfo(IUIForm uiForm)
            {
                Guard.Requires<ArgumentException>(uiForm != null, "UI form is invalid.");

                foreach (UIFormInfo uiFormInfo in m_UIFormInfos)
                {
                    if (uiFormInfo.UIForm == uiForm)
                    {
                        return uiFormInfo;
                    }
                }

                return null;
            }
        }
    }
}