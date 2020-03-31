/*
 *  Title 
 *
 *      层:  Action: 动作数据
 *
 *      Descripts:
 *          功能: 可以被执行的动作数据基类实现
 *          
 *      Date:
 *
 *      Version:0.1
 *
 *      Create Time: 
 *      
 *      Modify Recoder: 邓平
 */
namespace DpFrame.GOAP
{
    public abstract class ActionBase<TAction, TGoal> : IAction<TAction>
    {
        /// <summary>
        /// 当前东动作的标签
        /// </summary>
        public abstract TAction Label { get; }
        /// <summary>
        /// 当前动作的花费
        /// </summary>
        public abstract int Cost { get; }
        /// <summary>
        /// 当前动作的优先级
        /// </summary>
        public abstract int Priority { get; }
        /// <summary>
        /// 当前动作能否被打断
        /// </summary>
        public abstract bool CanInterruptiblePlan { get; }

        /// <summary>
        /// 执行动作的先决条件
        /// </summary>
        public IState Preconditions { get; private set; }

        /// <summary>
        /// 动作执行后的状态
        /// </summary>
        public IState Effects { get; private set; }

        /// <summary>
        /// 当前动作的代理对象
        /// </summary>
        protected IAgent<TAction, TGoal> _agent;

        /// <summary>
        /// 当前动作是否能够中断
        /// </summary>
        protected bool _interruptible;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="agent">当前动作的代理</param>
        public ActionBase(IAgent<TAction, TGoal> agent)
        {
            Preconditions = InitPreconditions();
            Effects = InitEffects();
            _agent = agent;
        }

        /// <summary>
        /// 初始化先决条件
        /// </summary>
        /// <returns></returns>
        protected abstract IState InitPreconditions();

        /// <summary>
        /// 初始化动作产生的影响
        /// </summary>
        /// <returns></returns>
        protected abstract IState InitEffects();


        /// <summary>
        /// 验证先决条件
        /// 逻辑:代理当前的状态  (是否包含当前状态的所有键值,且键值对应的值都相等)
        /// </summary>
        /// <returns>包含所有键值且值都相等  返回true，反之返回false</returns>
        public virtual bool VerifyPreconditions()
        {
            return _agent.AgentState.ContainState(Preconditions);
        }

    }
}
