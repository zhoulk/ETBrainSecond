using UnityEngine;
using LT;
using System.Threading;
using System.Threading.Tasks;
using LT.UI;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 普通项目初始化
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AppInit : Framework
    {
        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected override IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(base.GetBootstraps(), Bootstraps.GetBoostraps(this));
        }

        /// <summary>
        /// 在引导开始之前
        /// </summary>
        /// <param name="application">应用程序</param>
        protected override void BeforeBootstrap(LT.Application application)
        {
            base.BeforeBootstrap(application);

            if (DebugLevel == DebugLevel.Development)
            {
                application.OnResolving((bindData, instance) =>
                {
                    Debug.Log($"构建 <color=#00ff00>{bindData.Service}</color> 服务。");
                });
            }
        }

        /// <summary>
        /// 项目启动入口
        /// </summary>
        protected override void OnStartCompleted(StartCompletedEventArgs eventArgs)
        {
           
        }
    }
}