/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace LT.Fsm
{
    /// <summary>
    /// 有限状态机状态
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public abstract class FsmState<T> where T : class
    {
        /// <summary>
        /// 有限状态机
        /// </summary>
        protected IFsm<T> fsm;

        /// <summary>
        /// 状态持有者
        /// </summary>
        protected T owner;

        /// <summary>
        /// 需验证的条件
        /// </summary>
        private List<BindCondition> bindConditions = new List<BindCondition>();

        /// <summary>
        /// 有限状态机状态初始化时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public void OnInit(IFsm<T> fsm)
        {
            this.fsm = fsm;
            this.owner = fsm.Owner;

            this.OnInit();
        }

        /// <summary>
        /// 有限状态机状态初始化时调用。
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// 有限状态机状态进入时调用。
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// 有限状态机状态轮询时调用。
        /// </summary>
        public virtual void OnUpdate()
        {
            if (bindConditions.Count <= 0) return;

            foreach (var bind in bindConditions)
            {
                if (bind.Condition())
                {
                    ChangeState(bind.StateType);
                    break;
                }
            }
        }

        /// <summary>
        /// 有限状态机状态离开时调用。
        /// </summary>
        /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
        public virtual void OnLeave(bool isShutdown) { }

        /// <summary>
        /// 有限状态机状态销毁时调用。
        /// </summary>
        public virtual void OnDestroy()
        {
            bindConditions.Clear();
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        public void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public void ChangeState(Type stateType)
        {
            Guard.Verify<ArgumentNullException>(fsm == null, "FSM is invalid.");
            Guard.Verify<ArgumentNullException>(stateType == null, "State type is invalid.");

            fsm.ChangeState(stateType);
        }

        /// <summary>
        /// 当条件成立
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>绑定数据</returns>
        public IConditionSuccess Where(Func<bool> condition)
        {
            Guard.Verify<ArgumentException>(condition == null, "condition is invalid.");

            var bind = new BindCondition(condition);
            bindConditions.Add(bind);
            return bind;
        }

        /// <summary>
        /// 移除所有条件
        /// </summary>
        public void CleanupCoditions()
        {
            bindConditions.Clear();
        }

        /// <summary>
        /// 是否为任意状态
        /// </summary>
        public virtual bool IsAnyState => false;
    }
}