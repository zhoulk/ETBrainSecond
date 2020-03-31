/*
 *  Title 
 *
 *      层:  Agent: 代理接口
 *
 *      Descripts:
 *          功能: 负责连接各个类，主要逻辑在这里运行
 *          
 *      Date:
 *
 *      Version:0.1
 *
 *      Create Time: 
 *      
 *      Modify Recoder: 邓平
 */
using System;

namespace DpFrame.GOAP
{
    public abstract class AgentBase<TAction, TGoal> : IAgent<TAction, TGoal>
        where TAction : struct
        where TGoal : struct
    {
        public abstract bool IsAgentOver { get; }
        public IState AgentState { get; private set; }
        public IMaps<TAction, TGoal> Maps { get; protected set; }
        public IActionManager<TAction> ActionManager { get; protected set; }
        public IGoalManager<TGoal> GoalManager { get; private set; }
        public IPerformer Performer { get; private set; }

        private ITriggerManager _triggerManager;
        protected Action<IAgent<TAction, TGoal>, IMaps<TAction, TGoal>> _onInitGameData;


        public AgentBase(Action<IAgent<TAction, TGoal>, IMaps<TAction, TGoal>> onInitGameData)
        {
            //初始化所有的系统
            _onInitGameData = onInitGameData;
            DebugMsgBase.Instance = InitDebugMsg();
            AgentState = InitAgentState();
            Maps = InitMaps();
            ActionManager = InitActionManager();
            GoalManager = InitGoalManager();
            _triggerManager = InitTriggerManager();
            Performer = new Performer<TAction, TGoal>(this);
            
            AgentState.AddStateChangeListener(UpdateData);

            JudgeException(Maps, "Maps");
            JudgeException(ActionManager, "ActionManager");
            JudgeException(GoalManager, "GoalManager");
            JudgeException(_triggerManager, "_triggerManager");
        }

        private void JudgeException(object obj, string name)
        {
            if (obj == null)
            {
                DebugMsg.LogError("代理中" + name + "对象为空,请在代理子类中初始化该对象");
            }
        }

        /// <summary>
        /// 初始化代理状态
        /// </summary>
        /// <returns></returns>
        protected abstract IState InitAgentState();

        /// <summary>
        /// 初始化Action和Goal的映射
        /// </summary>
        /// <returns></returns>
        protected abstract IMaps<TAction, TGoal> InitMaps();
        protected abstract IActionManager<TAction> InitActionManager();
        protected abstract IGoalManager<TGoal> InitGoalManager();
        protected abstract ITriggerManager InitTriggerManager();
        protected abstract DebugMsgBase InitDebugMsg();

        /// <summary>
        /// 状态修改回调
        /// </summary>
        public void UpdateData()
        {
            if (IsAgentOver)
                return;

            if (ActionManager != null)
                ActionManager.UpdateData();

            if (GoalManager != null)
                GoalManager.UpdateData();

            Performer.UpdateData();
        }

        /// <summary>
        /// 帧函数
        /// </summary>
        public virtual void FrameFun()
        {
            if (IsAgentOver)
                return;

            if (_triggerManager != null)
                _triggerManager.FrameFun();

            if (ActionManager != null)
                ActionManager.FrameFun();
        }
    }
}
