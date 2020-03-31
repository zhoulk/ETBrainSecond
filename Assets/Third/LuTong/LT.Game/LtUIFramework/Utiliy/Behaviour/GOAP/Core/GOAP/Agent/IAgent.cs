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
namespace DpFrame.GOAP
{
    public interface IAgent<TAction, TGoal>
    {
        /// <summary>
        /// 当前代理结束
        /// </summary>
        bool IsAgentOver { get; }
        /// <summary>
        /// 当前状态
        /// </summary>
        IState AgentState { get; }
        /// <summary>
        /// 获取映射数据对象
        /// </summary>
        /// <returns></returns>
        IMaps<TAction, TGoal> Maps { get; }
        /// <summary>
        /// 获取动作管理类对象
        /// </summary>
        /// <returns></returns>
        IActionManager<TAction> ActionManager { get; }
        /// <summary>
        /// 获取目标管理类对象
        /// </summary>
        /// <returns></returns>
        IGoalManager<TGoal> GoalManager { get; }
        /// <summary>
        /// 计划执行器
        /// </summary>
        IPerformer Performer { get;}
        /// <summary>
        /// 更新数据函数
        /// </summary>
        void UpdateData();
        /// <summary>
        /// 帧函数
        /// </summary>
        void FrameFun();
    }
}
