/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/5
 * 模块描述：框架引导程序
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT
{
    /// <summary>
    /// 框架引导程序
    /// </summary>
    public abstract class Framework : MonoBehaviour
    {
        private Application m_Application;

        /// <summary>
        /// 调试等级
        /// </summary>
        public DebugLevel DebugLevel = DebugLevel.Development;

        /// <summary>
        /// Unity Application
        /// </summary>
        public IApplication Application => m_Application;

        /// <summary>
        /// 当框架启动完成时
        /// </summary>
        protected abstract void OnStartCompleted(StartCompletedEventArgs eventArgs);

        /// <summary>
        /// Unity Awake
        /// </summary>
        protected virtual void Awake()
        {
            if (App.That != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            m_Application = CreateApplication(DebugLevel);
            BeforeBootstrap(m_Application);
            m_Application.Bootstrap(GetBootstraps());
        }

        /// <summary>
        /// Unity Start
        /// </summary>
        protected virtual void Start()
        {
            m_Application.Init();
        }

        /// <summary>
        /// 创建新的Application实例
        /// </summary>
        /// <param name="debugLevel">调试等级</param>
        /// <returns>Application实例</returns>
        protected virtual Application CreateApplication(DebugLevel debugLevel)
        {
            return new UnityApplication(this)
            {
                DebugLevel = DebugLevel
            };
        }

        /// <summary>
        /// 在引导开始之前
        /// </summary>
        /// <param name="application">应用程序</param>
        protected virtual void BeforeBootstrap(Application application)
        {
            application.Dispatcher.AddListener<StartCompletedEventArgs>(OnStartCompleted);
        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected virtual IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(GetComponents<IBootstrap>(), Bootstraps.GetBoostraps(this));
        }

        /// <summary>
        /// 当被释放时
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (m_Application != null)
            {
                m_Application.Terminate();
            }
        }
    }
}