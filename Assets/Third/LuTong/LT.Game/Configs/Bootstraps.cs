/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/7
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LT;


namespace Game
{
    /// <summary>
    /// 框架默认的引导程序
    /// </summary>
    internal class Bootstraps
    {
        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <param name="component">Unity组件</param>
        /// <returns>引导程序</returns>
        public static IBootstrap[] GetBoostraps(Component component)
        {
            return new IBootstrap[]
            {
                new BootstrapTypeFinder(Assemblys.Assembly),
                new BootstrapProviderRegister(component, Providers.ServiceProviders),
            };
        }
    }
}