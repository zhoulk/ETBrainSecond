/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/7
 * 模块描述：项目注册的服务提供者
 * 
 * ------------------------------------------------------------------------------*/

using LT.MonoDriver;
using LT.Json;

namespace LT
{
    /// <summary>
    /// 框架默认的服务提供者
    /// <para>这里的服务提供者在框架启动时必定会被加载</para>
    /// </summary>
    internal class Providers
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {
                    new ProviderMonoDriver(),
                    new ProviderLog(),
                    new ProviderJson(),
                    new ProviderSyncContext(),
                };
            }
        }
    }
}