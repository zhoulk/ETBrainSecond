/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/5
 * 模块描述：Unity Application 的实现
 * 
 * ------------------------------------------------------------------------------*/

using System;
using UnityEngine;
using LT.Container;

namespace LT
{
    /// <summary>
    /// Unity 应用程序实现
    /// </summary>
    public class UnityApplication : Application
    {
        /// <summary>
        /// Mono Behaviour
        /// </summary>
        protected MonoBehaviour Behaviour { get; private set; }

        /// <summary>
        /// 构造一个 application
        /// </summary>
        /// <param name="behaviour">驱动器</param>
        public UnityApplication(MonoBehaviour behaviour)
        {
            if (behaviour == null)
            {
                return;
            }

            this.Singleton<MonoBehaviour>(() => behaviour).Alias<Component>();
            Behaviour = behaviour;
        }

        /// <summary>
        /// 初始化服务提供者
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="provider">服务提供者</param>
        /// <param name="force">为true则强制注册</param>
        public override void Register(IServiceProvider provider, bool force = false)
        {
            var component = provider as Component;
            if (component != null && !component)
            {
                throw new LogicException("Service providers inherited from MonoBehaviour only be registered mounting on the GameObject.");
            }

            base.Register(provider, force);
        }

        /// <summary>
        /// 是否是无法被构建的类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否是无法被构建的</returns>
        protected override bool IsUnableType(Type type)
        {
            return typeof(Component).IsAssignableFrom(type) || base.IsUnableType(type);
        }
    }
}