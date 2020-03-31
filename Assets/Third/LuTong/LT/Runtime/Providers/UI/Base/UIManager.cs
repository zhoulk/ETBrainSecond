/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/07/30
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using LT.MonoDriver;
using LT.ObjectPool;

namespace LT.UI
{
    /// <summary>
    /// 界面管理器
    /// </summary>
    internal sealed partial class UIManager : IUIManager, IUpdate
    {
        private readonly Dictionary<string, UIGroup> m_UIGroups;
        private IObjectPool<UIFormInstanceObject> m_InstancePool;
        private IUIFormHelper m_UIFormHelper;

        /// <summary>
        /// 构建界面管理器。
        /// </summary>
        public UIManager(IObjectPoolManager objectPoolManager)
        {
            m_UIGroups = new Dictionary<string, UIGroup>();
            m_UIFormHelper = null;

            m_InstancePool = objectPoolManager.CreateSingle<UIFormInstanceObject>("UI Pool");
        }

        /// <inheritdoc />
        public int UIGroupCount
        {
            get
            {
                return m_UIGroups.Count;
            }
        }

        /// <inheritdoc />
        public int InstanceCapacity
        {
            get
            {
                return m_InstancePool.Capacity;
            }
            set
            {
                m_InstancePool.Capacity = value;
            }
        }

        /// <inheritdoc />
        public float InstanceExpireTime
        {
            get
            {
                return m_InstancePool.ExpireTime;
            }
            set
            {
                m_InstancePool.ExpireTime = value;
            }
        }

        /// <inheritdoc />
        public int InstancePriority
        {
            get
            {
                return m_InstancePool.Priority;
            }
            set
            {
                m_InstancePool.Priority = value;
            }
        }

        /// <inheritdoc />
        public void Update()
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.Update();
            }
        }

        /// <inheritdoc />
        internal void Shutdown()
        {
            CloseAllLoadedUIForms();
            m_UIGroups.Clear();
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {
            Guard.Verify<ArgumentException>(uiFormHelper == null, "UI form helper is invalid.");

            this.m_UIFormHelper = uiFormHelper;
        }

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            Guard.NotEmptyOrNull(uiGroupName, "UI group name is invalid.");
            return m_UIGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            Guard.NotEmptyOrNull(uiGroupName, "UI group name is invalid.");

            UIGroup uiGroup;
            if (m_UIGroups.TryGetValue(uiGroupName, out uiGroup))
            {
                return uiGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            int index = 0;
            IUIGroup[] results = new IUIGroup[m_UIGroups.Count];
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results[index++] = uiGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面组。</returns>
        public IUIGroup AddUIGroup(string uiGroupName)
        {
            return AddUIGroup(uiGroupName, 0);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <returns>界面组。</returns>
        public IUIGroup AddUIGroup(string uiGroupName, int uiGroupDepth)
        {
            Guard.NotEmptyOrNull(uiGroupName, "UI group name is invalid.");
            Guard.Verify<ArgumentException>(m_UIFormHelper == null, "UI form helper is invalid.");

            var group = (UIGroup)GetUIGroup(uiGroupName);

            if (group != null)
            {
                return group;
            }

            group = new UIGroup(uiGroupName, uiGroupDepth, m_UIFormHelper.CreateUIGroupHelper(uiGroupName));
            m_UIGroups.Add(uiGroupName, group);
            return group;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            Guard.NotEmptyOrNull(uiFormAssetName, "UI form asset name is invalid.");

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                if (uiGroup.Value.HasUIForm(uiFormAssetName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm GetUIForm(string uiFormAssetName)
        {
            Guard.NotEmptyOrNull(uiFormAssetName, "UI form asset name is invalid.");

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                IUIForm uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public IUIForm[] GetAllLoadedUIForms()
        {
            List<IUIForm> results = new List<IUIForm>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results.AddRange(uiGroup.Value.GetAllUIForms());
            }

            return results.ToArray();
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUIForm(IUIForm uiForm)
        {
            if (uiForm == null)
            {
                return false;
            }

            return HasUIForm(uiForm.UIFormAssetName);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName)
        {
            OpenUIForm(uiFormAssetName, uiGroupName, 0, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, int uiGroupDepth)
        {
            OpenUIForm(uiFormAssetName, uiGroupName, uiGroupDepth, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
        {
            OpenUIForm(uiFormAssetName, uiGroupName, 0, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm)
        {
            OpenUIForm(uiFormAssetName, uiGroupName, 0, pauseCoveredUIForm, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, int uiGroupDepth, object userData)
        {
            OpenUIForm(uiFormAssetName, uiGroupName, uiGroupDepth, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, int uiGroupDepth, bool pauseCoveredUIForm, object userData)
        {
            Guard.Verify<ArgumentException>(m_UIFormHelper == null, "You must set UI form helper first.");
            Guard.NotEmptyOrNull(uiFormAssetName, "UI form asset name is invalid.");
            Guard.NotEmptyOrNull(uiGroupName, "UI group name is invalid.");

            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);

            if (uiGroup == null)
            {
                uiGroup = (UIGroup)AddUIGroup(uiGroupName, uiGroupDepth);
            }

            if (HasUIForm(uiFormAssetName))
            {
                return;
            }

            UIFormInstanceObject uiFormInstanceObject = m_InstancePool.Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
                // 把资源加载抽到这里，为后续异步加载资源与实例化分开处理
                var uiFormInstance = m_UIFormHelper.InstantiateUIForm(ResourceUtils.Load(uiFormAssetName));
                uiFormInstanceObject = new UIFormInstanceObject(uiFormAssetName, uiFormInstance, m_UIFormHelper);
                m_InstancePool.Register(uiFormInstanceObject, true);

                InnerOpenUIForm(uiFormAssetName, uiGroup, uiFormInstanceObject.Target, pauseCoveredUIForm, true, userData);
            }
            else
            {
                InnerOpenUIForm(uiFormAssetName, uiGroup, uiFormInstanceObject.Target, pauseCoveredUIForm, false, userData);
            }
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiFormAssetName">要关闭界面的名称。</param>
        /// <param name="isRelease">是否释放</param>
        public void CloseUIForm(string uiFormAssetName, bool isRelease = false)
        {
            CloseUIForm(uiFormAssetName, null, isRelease);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiFormAssetName">要关闭界面的名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isRelease">是否释放</param>
        public void CloseUIForm(string uiFormAssetName, object userData, bool isRelease = false)
        {
            IUIForm uiForm = GetUIForm(uiFormAssetName);
            Guard.Verify<ArgumentException>(uiForm == null, $"Can not find UI form '{uiFormAssetName}'.");

            CloseUIForm(uiForm, userData, isRelease);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="isRelease">是否释放</param>
        public void CloseUIForm(IUIForm uiForm, bool isRelease = false)
        {
            CloseUIForm(uiForm, null, isRelease);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(IUIForm uiForm, object userData, bool isRelease = false)
        {
            Guard.Verify<ArgumentException>(uiForm == null, $"UI form is invalid.");

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;

            Guard.Verify<ArgumentException>(uiGroup == null, $"UI group is invalid.");

            // 先关闭前台界面，再激活后台界面
            uiGroup.RemoveUIForm(uiForm);
            uiForm.OnClose(userData);
            uiGroup.Refresh();

            if (isRelease)
            {
                m_InstancePool.Release(uiForm.UIFormInstance);
            }
            else
            {
                m_InstancePool.Unspawn(uiForm.UIFormInstance);
            }
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// <param name="isRelease">是否释放</param>
        /// </summary>
        public void CloseAllLoadedUIForms(bool isRelease = false)
        {
            CloseAllLoadedUIForms(null, isRelease);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isRelease">是否释放</param>
        public void CloseAllLoadedUIForms(object userData, bool isRelease = false)
        {
            IUIForm[] uiForms = GetAllLoadedUIForms();
            foreach (IUIForm uiForm in uiForms)
            {
                if (!HasUIForm(uiForm.UIFormAssetName))
                {
                    continue;
                }

                CloseUIForm(uiForm, userData, isRelease);
            }
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        public void RefocusUIForm(IUIForm uiForm)
        {
            RefocusUIForm(uiForm, null);
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RefocusUIForm(IUIForm uiForm, object userData)
        {
            Guard.Verify<ArgumentException>(uiForm == null, $"UI form is invalid.");

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;

            Guard.Verify<ArgumentException>(uiGroup == null, $"UI group is invalid.");

            uiGroup.RefocusUIForm(uiForm, userData);
            uiGroup.Refresh();
            uiForm.OnRefocus(userData);
        }

        /// <summary>
        /// 设置界面实例是否被加锁。
        /// </summary>
        /// <param name="uiFormInstance">要设置是否被加锁的界面实例。</param>
        /// <param name="locked">界面实例是否被加锁。</param>
        public void SetUIFormInstanceLocked(object uiFormInstance, bool locked)
        {
            Guard.Verify<ArgumentException>(uiFormInstance == null, $"UI form instance is invalid.");
            m_InstancePool.SetLocked(uiFormInstance, locked);
        }

        /// <summary>
        /// 设置界面实例的优先级。
        /// </summary>
        /// <param name="uiFormInstance">要设置优先级的界面实例。</param>
        /// <param name="priority">界面实例优先级。</param>
        public void SetUIFormInstancePriority(object uiFormInstance, int priority)
        {
            Guard.Verify<ArgumentException>(uiFormInstance == null, $"UI form instance is invalid.");
            m_InstancePool.SetPriority(uiFormInstance, priority);
        }

        private IUIForm InnerOpenUIForm(string uiFormAssetName, UIGroup uiGroup, object uiFormInstance, bool pauseCoveredUIForm, bool isNewInstance, object userData)
        {
            IUIForm uiForm = m_UIFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);

            Guard.Verify<ArgumentException>(uiForm == null, $"Can not create UI form in helper.");

            uiForm.OnInit(uiFormAssetName, uiGroup, pauseCoveredUIForm, isNewInstance, userData);

            uiGroup.AddUIForm(uiForm);
            uiGroup.RefreshOnlyOlds();
            uiForm.OnOpen(userData);
            uiGroup.RefreshOnlyNew();

            return uiForm;
        }
    }
}