/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/07/30
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.UI
{
    /// <summary>
    /// 界面管理器接口。
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        int UIGroupCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        int InstanceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        float InstanceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        int InstancePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        void SetUIFormHelper(IUIFormHelper uiFormHelper);

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        bool HasUIGroup(string uiGroupName);

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        IUIGroup GetUIGroup(string uiGroupName);

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        IUIGroup[] GetAllUIGroups();

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面组。</returns>
        IUIGroup AddUIGroup(string uiGroupName);

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <returns>界面组。</returns>
        IUIGroup AddUIGroup(string uiGroupName, int uiGroupDepth);

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        bool HasUIForm(string uiFormAssetName);

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIForm GetUIForm(string uiFormAssetName);

        #region 暂移除所有异步加载方式
        ///// <summary>
        ///// 获取所有已加载的界面。
        ///// </summary>
        ///// <returns>所有已加载的界面。</returns>
        //IUIForm[] GetAllLoadedUIForms();

        ///// <summary>
        ///// 获取所有正在加载界面的序列编号。
        ///// </summary>
        ///// <returns>所有正在加载界面的序列编号。</returns>
        //int[] GetAllLoadingUIFormSerialIds();

        ///// <summary>
        ///// 是否正在加载界面。
        ///// </summary>
        ///// <param name="serialId">界面序列编号。</param>
        ///// <returns>是否正在加载界面。</returns>
        //bool IsLoadingUIForm(int serialId);

        ///// <summary>
        ///// 是否正在加载界面。
        ///// </summary>
        ///// <param name="uiFormAssetName">界面资源名称。</param>
        ///// <returns>是否正在加载界面。</returns>
        //bool IsLoadingUIForm(string uiFormAssetName);
        #endregion

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <returns>界面是否合法。</returns>
        bool IsValidUIForm(IUIForm uiForm);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        void OpenUIForm(string uiFormAssetName, string uiGroupName = "Default");

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        void OpenUIForm(string uiFormAssetName, string uiGroupName, int uiGroupDepth);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OpenUIForm(string uiFormAssetName, string uiGroupName, object userData);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        void OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OpenUIForm(string uiFormAssetName, string uiGroupName, int uiGroupDepth, object userData);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OpenUIForm(string uiFormAssetName, string uiGroupName, int uiGroupDepth, bool pauseCoveredUIForm, object userData);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiFormAssetName">要关闭界面的名称。</param>
        /// <param name="isRelease">是否释放</param>
        void CloseUIForm(string uiFormAssetName, bool isRelease = false);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiFormAssetName">要关闭界面的名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isRelease">是否释放</param>
        void CloseUIForm(string uiFormAssetName, object userData, bool isRelease = false);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="isRelease">是否释放</param>
        void CloseUIForm(IUIForm uiForm, bool isRelease = false);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isRelease">是否释放</param>
        void CloseUIForm(IUIForm uiForm, object userData, bool isRelease = false);

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="isRelease">是否释放</param>
        void CloseAllLoadedUIForms(bool isRelease = false);

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isRelease">是否释放</param>
        void CloseAllLoadedUIForms(object userData, bool isRelease = false);

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        void RefocusUIForm(IUIForm uiForm);

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void RefocusUIForm(IUIForm uiForm, object userData);

        /// <summary>
        /// 设置界面实例是否被加锁。
        /// </summary>
        /// <param name="uiFormInstance">要设置是否被加锁的界面实例。</param>
        /// <param name="locked">界面实例是否被加锁。</param>
        void SetUIFormInstanceLocked(object uiFormInstance, bool locked);

        /// <summary>
        /// 设置界面实例的优先级。
        /// </summary>
        /// <param name="uiFormInstance">要设置优先级的界面实例。</param>
        /// <param name="priority">界面实例优先级。</param>
        void SetUIFormInstancePriority(object uiFormInstance, int priority);
    }
}