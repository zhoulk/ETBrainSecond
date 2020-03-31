/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2020/01/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.Threading;

namespace LT
{
    /// <summary>
    /// 同步上下文
    /// </summary>
    public sealed class LTSyncContext : Facade<ISyncContext>
    {
        /// <summary>
        /// 将方法推到主线程执行
        /// </summary>
        /// <param name="callback">在主线程调用的方法</param>
        /// <param name="state">携带的参数</param>
        public static void Post(SendOrPostCallback callback, object state = null)
        {
            That.Post(callback, state);
        }
    }
}