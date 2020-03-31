/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/7
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LT
{
    /// <summary>
    /// 服务提供者引导程序
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class BootstrapProviderRegister : IBootstrap
    {
        /// <summary>
        /// 服务提供者列表
        /// </summary>
        private readonly IServiceProvider[] providers = new IServiceProvider[0];

        /// <summary>
        /// Unity根组件
        /// </summary>
        private readonly Component component;

        /// <summary>
        /// 构建一个服务提供者引导程序
        /// </summary>
        /// <param name="component">Unity根组件</param>
        /// <param name="serviceProviders">服务提供者列表</param>
        public BootstrapProviderRegister(Component component, IServiceProvider[] serviceProviders = null)
        {
            providers = Arr.Merge(providers, serviceProviders);

            this.component = component;
        }

        /// <summary>
        /// 引导程序接口
        /// </summary>
        public void Bootstrap()
        {
            LoadCodeProvider();
            LoadUnityComponentProvider();
        }

        /// <summary>
        /// 加载以代码形式提供的服务提供者
        /// </summary>
        private void LoadCodeProvider()
        {
            RegisterProviders(providers);
        }

        /// <summary>
        /// 加载Unity组件的服务提供者
        /// </summary>
        private void LoadUnityComponentProvider()
        {
            if (!component)
            {
                return;
            }

            RegisterProviders(component.GetComponentsInChildren<IServiceProvider>());
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="providers">服务提供者</param>
        private static void RegisterProviders(IEnumerable<IServiceProvider> providers)
        {
            foreach (var provider in providers)
            {
                if (provider == null)
                {
                    continue;
                }

                if (!App.IsRegistered(provider))
                {
                    App.Register(provider);
                }
            }
        }
    }
}
