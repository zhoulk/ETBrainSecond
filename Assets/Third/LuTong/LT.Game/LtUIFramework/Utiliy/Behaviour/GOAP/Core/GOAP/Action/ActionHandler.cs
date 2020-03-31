/*
 *  Title 
 *
 *      层:  Action: 动作执行
 *
 *      Descripts:
 *          功能: 可以被执行的动作执行基类实现
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
using System.Threading.Tasks;

namespace DpFrame.GOAP
{
    public abstract class ActionHandlerBase<TAction, TGoal> : IActionHandler<TAction>
    {
        /// <summary>
        /// 动作
        /// </summary>
        public IAction<TAction> Action { get; private set; }

        /// <summary>
        /// 当前动作标签
        /// </summary>
        public TAction Label
        {
            get { return Action.Label; }
        }

        /// <summary>
        /// 当前动作执行状态
        /// </summary>
        public ActionExcuteState ExcuteState { get; private set; }

        /// <summary>
        /// 当前代理
        /// </summary>
        protected IAgent<TAction, TGoal> _agent;

        /// <summary>
        /// 当前动作数据
        /// </summary>
        private IAction<TAction> action;

        /// <summary>
        /// 动作完成回调
        /// </summary>
        protected System.Action _onFinishAction;
        /// <summary>
        /// 动作 和 目标 映射表
        /// </summary>
        protected IMaps<TAction, TGoal> _maps;

        private static int _id;

        /// <summary>
        /// 当前动作 ID
        /// </summary>
        protected int ID { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="agent">代理</param>
        /// <param name="maps">动作 和 目标 映射</param>
        /// <param name="action">动作数据</param>
        public ActionHandlerBase(IAgent<TAction, TGoal> agent, IMaps<TAction, TGoal> maps, IAction<TAction> action)
        {
            // 使用静态 _id累加 给ID赋值 防止ID重复
            ID = _id ++;
            _agent = agent;
            _maps = maps;
            Action = action;
            //当前动作状态为 初始化
            ExcuteState = ActionExcuteState.INIT;
            _onFinishAction = null;
        }

        /// <summary>
        /// 设置当前代理的状态
        /// 把所提供状态的所有数据覆盖到当前状态
        /// </summary>
        /// <param name="state"></param>
        private void SetAgentData(IState state)
        {
            _agent.AgentState.Set(state);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetAgentState<TKey>(TKey key,bool value)
        {
            _agent.AgentState.Set(key.ToString(),value);
        }

        protected async void OnComplete(float delayTime = 0)
        {
            if(ExcuteState == ActionExcuteState.EXIT)
                return;

            await Task.Delay(TimeSpan.FromSeconds(delayTime));

            ExcuteState = ActionExcuteState.EXIT;

            DebugMsg.Log("------设置"+Label+"影响");
            if (Action.Effects != null)
                SetAgentData(Action.Effects);

            if (_onFinishAction != null)
                _onFinishAction();
        }

        /// <summary>
        /// 验证当动作先决条件
        /// </summary>
        /// <returns></returns>
        public virtual bool CanPerformAction()
        {
            return Action.VerifyPreconditions();
        }

        /// <summary>
        /// 添加动作完成回调
        /// </summary>
        /// <param name="onFinishAction"></param>
        public void AddFinishCallBack(Action onFinishAction)
        {
            _onFinishAction = onFinishAction;
        }


        protected virtual TClass GetGameData<TKey,TClass>(TKey key) where TKey : struct where TClass : class
        {
            return _maps.GetGameData<TKey, TClass>(key);
        }
        protected virtual TValue GetGameDataValue<TKey, TValue>(TKey key) where TKey : struct where TValue : struct 
        {
            return _maps.GetGameDataValue<TKey, TValue>(key);
        }
        protected virtual object GetGameData<TKey>(TKey key) where TKey : struct
        {
            return _maps.GetGameData(key);
        }

        public virtual void Enter()
        {
            ExcuteState = ActionExcuteState.ENTER;
        }

        public virtual void Execute()
        {
            ExcuteState = ActionExcuteState.EXCUTE;
        }

        public virtual void Exit()
        {
            if (ExcuteState != ActionExcuteState.EXIT)
            {
                OnComplete();
            }
        }

    }
}
