/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT;
using System;

namespace LT.Fsm
{
    /// <summary>
    /// 有限状态机管理器。
    /// </summary>
    public interface IFsmManager
    {
        /// <summary>
        /// 获取有限状态机数量。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <param name="owner">状态机持有者</param>
        /// <returns>持有返回true</returns>
        bool HasFsm(object owner);

        /// <summary>
        /// 创建逻辑有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <returns>要创建的有限状态机</returns>
        IFsm<T> CreateLogicFsm<T>(T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// 逻辑层轮询
        /// </summary>
        void OnLogicUpdate();

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="owner">有限状态机持有者。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        bool DestroyFsm(object owner);
    }
}