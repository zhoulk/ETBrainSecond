/*
 *    描述:
 *          1.UIMgr 外观
 *
 *    开发人: 邓平
 */

namespace LtFramework.UI
{
    public static class UIMgr
    {
        /// <summary>
        /// 获得UI
        /// </summary>
        /// <param UIName="uiFormName"></param>
        /// <returns></returns>
        public static IBaseUIForm GetUI(string uiFormName)
        {
            return UIMonoManager.GetUI(uiFormName);
        }

        /// <summary>
        /// 获得UI
        /// </summary>
        /// <typeparam UIName="TBaseUI"></typeparam>
        /// <returns></returns>
        public static TBaseUI GetUI<TBaseUI>() where TBaseUI : IBaseUIForm
        {
            return UIMonoManager.GetUI<TBaseUI>();
        }

        /// <summary>
        /// 判断当前窗口是否打开
        /// </summary>
        /// <param name="name">窗口名字</param>
        /// <returns></returns>
        public static bool IsOpenUI(string name)
        {
            return UIMonoManager.IsOpenUI(name);
        }

        /// <summary>
        /// 得到当前打开的NormalUI
        /// </summary>
        /// <param name="name">UI名字</param>
        /// <returns></returns>
        public static IBaseUIForm GetCurrentOpenUI(string name)
        {
            return UIMonoManager.GetCurrentOpenUI(name);
        }

        /// <summary>
        /// 获取当前显示的窗体
        /// </summary>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllCurrentOpenUI => UIMonoManager.GetAllCurrentOpenUI;

        /// <summary>
        /// 获取当前没有被冻结或关闭的UI
        /// </summary>
        public static IBaseUIForm[] GetEnableCtrlUI => UIMonoManager.GetEnableCtrlUI;

        /// <summary>
        /// 是否存在 NewInstanceUI
        /// </summary>
        /// <param name="name">UI名字</param>
        /// <returns></returns>
        public static bool ExistNewInstanceUI(string name)
        {
            return UIMonoManager.ExistNewInstanceUI(name);
        }

        /// <summary>
        /// 获取所有 Open NewInstanceUI
        /// </summary>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllNewInstanceOpenUI()
        {
            return UIMonoManager.GetAllNewInstanceOpenUI();
        }

        /// <summary>
        /// 获取所有 Close NewInstanceUI
        /// </summary>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllNewInstanceCloseUI()
        {
            return UIMonoManager.GetAllNewInstanceCloseUI();
        }


        /// <summary>
        /// 得到所有 NewInstanceUI 窗口
        /// </summary>
        /// <typeparam name="TBaseUI">窗口类型</typeparam>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllNewInstanceOpenUI<TBaseUI>() where TBaseUI : IBaseUIForm
        {
            return UIMonoManager.GetAllNewInstanceOpenUI<TBaseUI>();

        }

        /// <summary>
        /// 得到所有 NewInstanceUI 窗口
        /// </summary>
        /// <typeparam name="TBaseUI">窗口类型</typeparam>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllNewInstanceCloseUI<TBaseUI>()
        {
            return UIMonoManager.GetAllNewInstanceCloseUI<TBaseUI>();
        }

        /// <summary>
        /// 销毁所有 关闭的普通窗体
        /// </summary>
        public static void DestoryAllNoramlUI()
        {
            UIMonoManager.DestoryAllNoramlUI();
        }

        /// <summary>
        /// 销毁所有 关闭的NewInstance窗体
        /// </summary>
        public static void DestoryAllNewInstanceUI()
        {
            UIMonoManager.DestoryAllNewInstanceUI();
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="uiName">窗口名字</param>
        public static void PreLoadUI(string uiName)
        {
            UIMonoManager.Instance.PreLoadUI(uiName);
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <typeparam name="TBaseUIForm">窗口类型</typeparam>
        public static void PreLoadUI<TBaseUIForm>() where TBaseUIForm : IBaseUIForm
        {
            UIMonoManager.Instance.PreLoadUI<TBaseUIForm>();
        }
    }

}
