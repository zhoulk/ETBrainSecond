/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/25
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    /// <summary>
    /// 线程同步服务
    /// </summary>
    public class ProviderSyncContext : IServiceProvider
    {
        /// <summary>
        /// 注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<ISyncContext, OneThreadSyncContext>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
            App.Make<ISyncContext>();
        }
    }
}