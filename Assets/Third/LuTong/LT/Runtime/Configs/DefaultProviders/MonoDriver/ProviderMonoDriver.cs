/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/4
 * 模块描述： Mono驱动器服务
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.MonoDriver
{
    /// <summary>
    /// Mono驱动器服务
    /// </summary>
    public sealed class ProviderMonoDriver : IServiceProvider
    {
        /// <summary>
        /// 注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<IMonoDriver, MonoDriver>();
        }

        /// <summary>
        /// 初始化服务，且标记优先级
        /// </summary>
        public void Init()
        {
            App.Make<IMonoDriver>();
        }
    }
}