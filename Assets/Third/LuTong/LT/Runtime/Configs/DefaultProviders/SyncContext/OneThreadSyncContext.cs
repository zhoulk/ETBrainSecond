/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/25
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Concurrent;
using System.Threading;
using LT.MonoDriver;

namespace LT
{
    /// <summary>
    /// 提供在各种同步模型中传播同步上下文的基本功能。
    /// </summary>
    public class OneThreadSyncContext : SynchronizationContext, ISyncContext, IUpdate
    {
        private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

        /// <summary>
        /// 构建上下文
        /// </summary>
        public OneThreadSyncContext()
        {
            SetSynchronizationContext(this);
        }

        /// <summary>
        /// 将方法推到主线程执行
        /// </summary>
        /// <param name="callback">在主线程调用的方法</param>
        /// <param name="state">携带的参数</param>
        public override void Post(SendOrPostCallback callback, object state = null)
        {
            lock (queue)
            {
                queue.Enqueue(() => callback(state));
            }
        }

        public void Update()
        {
            while (true)
            {
                lock (queue)
                {
                    if (!queue.TryDequeue(out Action action))
                    {
                        return;
                    }

                    action();
                }
            }
        }
    }
}