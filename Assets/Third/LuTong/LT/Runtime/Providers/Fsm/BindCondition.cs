/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/11/20
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT.Fsm
{
    /// <summary>
    /// 当条件成立
    /// </summary>
    public interface IConditionSuccess
    {
        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        void ChangeState<TState>();
    }

    /// <summary>
    /// 条件绑定
    /// </summary>
    public class BindCondition : IConditionSuccess
    {
        /// <summary>
        /// 构建条件绑定
        /// </summary>
        /// <param name="condition"></param>
        public BindCondition(Func<bool> condition)
        {
            Condition = condition;
        }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StateType
        {
            get;
            private set;
        }

        /// <summary>
        /// 条件
        /// </summary>
        public Func<bool> Condition
        {
            get;
            private set;
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        public void ChangeState<TState>()
        {
            StateType = typeof(TState);
        }
    }
}