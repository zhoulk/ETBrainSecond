/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace LT.Fsm
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
	internal sealed class Fsm<T> : IFsm<T> where T : class
    {
        private readonly T owner;
        private FsmState<T> currentState;
        private float currentStateTime;
        private bool isDestroyed;
        private readonly Dictionary<string, FsmState<T>> fsmStates;
        private readonly Dictionary<string, FsmState<T>> anyStates;

        /// <summary>
        /// 初始化有限状态机的新实例。
        /// </summary>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="fsmStates">有限状态机状态集合。</param>
        public Fsm(T owner, params FsmState<T>[] fsmStates)
        {
            Guard.Requires<ArgumentException>(fsmStates.Length > 0);
            Guard.Requires<ArgumentException>(fsmStates != null, "FSM states is invalid.");

            this.owner = owner;
            this.fsmStates = new Dictionary<string, FsmState<T>>();
            this.anyStates = new Dictionary<string, FsmState<T>>();

            foreach (FsmState<T> state in fsmStates)
            {
                Guard.Requires<ArgumentException>(state != null, "FSM states is invalid.");

                string stateName = state.GetType().FullName;
                if (this.fsmStates.ContainsKey(stateName))
                {
                    throw new LogicException(string.Format("FSM '{0}' state '{1}' is already exist.", Utility.Text.GetFullName<T>(string.Empty), stateName));
                }

                // 另存任意状态
                if (state.IsAnyState)
                {
                    anyStates.Add(stateName, state);
                }

                this.fsmStates.Add(stateName, state);
                state.OnInit(this);
            }

            currentStateTime = 0f;
            isDestroyed = false;
            currentState = null;
        }

        /// <summary>
        /// 获取有限状态机持有者。
        /// </summary>
        public T Owner => owner;

        /// <summary>
        /// 获取有限状态机中状态的数量。
        /// </summary>
        public int FsmStateCount => fsmStates.Count;

        /// <summary>
        /// 获取有限状态机是否正在运行。
        /// </summary>
        public bool IsRunning => currentState != null;

        /// <summary>
        /// 获取有限状态机是否被销毁。
        /// </summary>
        public bool IsDestroyed => isDestroyed;

        /// <summary>
        /// 获取当前有限状态机状态。
        /// </summary>
        public FsmState<T> CurrentState => currentState;

        /// <summary>
        /// 获取当前有限状态机状态持续时间。
        /// </summary>
        public float CurrentStateTime => currentStateTime;

        /// <summary>
        /// 开始有限状态机。
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机状态类型。</typeparam>
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new LogicException("FSM is running, can not start again.");
            }

            TState state = GetState<TState>();
            Guard.Requires<ArgumentException>(state != null, "FSM states is invalid.");

            currentStateTime = 0f;
            currentState = state;
            currentState.OnEnter();
        }

        /// <summary>
        /// 开始有限状态机。
        /// </summary>
        /// <param name="stateType">要开始的有限状态机状态类型。</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new LogicException("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState(stateType);
            Guard.Requires<ArgumentException>(state != null, "FSM states is invalid.");

            currentStateTime = 0f;
            currentState = state;
            currentState.OnEnter();
        }

        /// <summary>
        /// 是否存在有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要检查的有限状态机状态类型。</typeparam>
        /// <returns>是否存在有限状态机状态。</returns>
        public bool HasState<TState>() where TState : FsmState<T>
        {
            return HasState(typeof(TState));
        }

        /// <summary>
        /// 是否当前正在运行该状态
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        public bool IsRunningState<TState>() where TState : FsmState<T>
        {
            if (currentState.GetType() == typeof(TState))
                return true;

            return false;
        }

        /// <summary>
        /// 是否存在有限状态机状态。
        /// </summary>
        /// <param name="stateType">要检查的有限状态机状态类型。</param>
        /// <returns>是否存在有限状态机状态。</returns>
        public bool HasState(Type stateType)
        {
            if (fsmStates.ContainsKey(stateType.FullName))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机状态类型。</typeparam>
        /// <returns>要获取的有限状态机状态。</returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            if (fsmStates.TryGetValue(typeof(TState).FullName, out FsmState<T> state))
            {
                return (TState)state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型。</param>
        /// <returns>要获取的有限状态机状态。</returns>
        public FsmState<T> GetState(Type stateType)
        {
            if (fsmStates.TryGetValue(stateType.FullName, out FsmState<T> state))
            {
                return state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机的所有状态。
        /// </summary>
        /// <returns>有限状态机的所有状态。</returns>
        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] results = new FsmState<T>[fsmStates.Count];
            foreach (KeyValuePair<string, FsmState<T>> state in fsmStates)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">有限状态机状态</param>
        public void AddState(FsmState<T> state)
        {
            string stateName = state.GetType().FullName;
            if (this.fsmStates.ContainsKey(stateName))
            {
                throw new LogicException(string.Format("FSM '{0}' state '{1}' is already exist.", Utility.Text.GetFullName<T>(string.Empty), stateName));
            }

            if (state.IsAnyState)
            {
                anyStates.Add(stateName, state);
            }

            this.fsmStates.Add(stateName, state);
            state.OnInit(this);
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state">有限状态机状态</param>
        public void RemoveState(FsmState<T> state)
        {
            string stateName = state.GetType().FullName;
            if (fsmStates.ContainsKey(stateName))
            {
                fsmStates.Remove(stateName);
            }
        }

        /// <inheritdoc />
        public void OnUpdate()
        {
            if (currentState == null) return;

            if (!currentState.IsAnyState)
                currentState.OnUpdate();

            foreach (var anyState in anyStates.Values)
            {
                anyState.OnUpdate();
            }
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
            Guard.Verify<ArgumentException>(currentState == null, "Current states is invalid.");

            FsmState<T> state = GetState(stateType);

            Guard.Verify<ArgumentException>(state == null, $"{stateType.Name} is not init.");

            currentState.OnLeave(false);
            currentStateTime = 0f;
            currentState = state;
            currentState.OnEnter();
        }

        /// <summary>
        /// 关闭并清理有限状态机。
        /// </summary>
        public void Shutdown()
        {
            if (currentState != null)
            {
                currentState.OnLeave(true);
                currentState = null;
                currentStateTime = 0f;
            }

            foreach (KeyValuePair<string, FsmState<T>> state in fsmStates)
            {
                state.Value.OnDestroy();
            }

            fsmStates.Clear();
            anyStates.Clear();
            isDestroyed = true;
        }
    }
}