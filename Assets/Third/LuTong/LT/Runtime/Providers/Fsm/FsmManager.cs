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
using LT;
using LT.MonoDriver;

namespace LT.Fsm
{
    /// <summary>
    /// 有限状态机管理器
    /// </summary>
	internal sealed class FsmManager : IFsmManager, IUpdate
    {
        private readonly Dictionary<object, IFsm> m_Fsms;
        private readonly List<IFsm> m_LogicFsms;
        private readonly List<IFsm> m_CommonFsms;

        /// <summary>
        /// 初始化有限状态机管理器的新实例。
        /// </summary>
        public FsmManager()
        {
            m_Fsms = new Dictionary<object, IFsm>();
            m_CommonFsms = new List<IFsm>();
            m_LogicFsms = new List<IFsm>();
        }

        /// <inheritdoc />
        public void Update()
        {
            foreach (var fsm in m_CommonFsms)
            {
                if (fsm.IsDestroyed)
                {
                    continue;
                }

                fsm.OnUpdate();
            }
        }

        /// <summary>
        /// 逻辑层轮询
        /// </summary>
        public void OnLogicUpdate()
        {
            foreach (var fsm in m_LogicFsms)
            {
                if (fsm.IsDestroyed)
                {
                    continue;
                }

                fsm.OnUpdate();
            }
        }

        /// <summary>
        /// 获取有限状态机数量。
        /// </summary>
        public int Count => m_Fsms.Count;

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <param name="owner">状态机持有者</param>
        /// <returns>持有返回true</returns>
        public bool HasFsm(object owner)
        {
            return m_Fsms.ContainsKey(owner);
        }

        /// <summary>
        /// 创建逻辑有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <returns>要创建的有限状态机</returns>
        public IFsm<T> CreateLogicFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            var fsm = InnerCreateFsm(owner, states);
            m_LogicFsms.Add(fsm);
            return fsm;
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            var fsm = InnerCreateFsm(owner, states);
            m_CommonFsms.Add(fsm);
            return fsm;
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        private IFsm<T> InnerCreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            if (m_Fsms.ContainsKey(owner))
            {
                throw new RuntimeException(string.Format("Already exist FSM '{0}'.{1}", Utility.Text.GetFullName<T>(string.Empty), owner));
            }

            Fsm<T> fsm = new Fsm<T>(owner, states);
            m_Fsms.Add(owner, fsm);

            return fsm;
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="owner">有限状态机持有者。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(object owner)
        {
            Guard.Requires<ArgumentException>(owner != null, "FSM owner is invaild.");

            if (m_Fsms.TryGetValue(owner, out IFsm fsm))
            {
                fsm.Shutdown();
                m_Fsms.Remove(owner);

                if (m_CommonFsms.Contains(fsm)) m_CommonFsms.Remove(fsm);
                if (m_LogicFsms.Contains(fsm)) m_LogicFsms.Remove(fsm);

                return true;
            }

            LTLog.Debug($"owner is not has fsm.{owner.ToString()}");

            return false;
        }
    }
}