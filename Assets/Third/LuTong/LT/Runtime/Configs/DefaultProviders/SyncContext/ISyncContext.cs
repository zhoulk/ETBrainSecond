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

namespace LT
{
    /// <summary>
    /// 线程上下文接口
    /// </summary>
    public interface ISyncContext
    {
        /// <summary>
        /// 将方法推到主线程执行
        /// </summary>
        /// <param name="callback">在主线程调用的方法</param>
        /// <param name="state">携带的参数</param>
        void Post(SendOrPostCallback callback, object state = null);
    }
}