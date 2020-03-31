/*
 *  Title 
 *
 *      层:  Action: 动作执行
 *
 *      Descripts:
 *          功能: 可以被执行的动作执行接口
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
    /// <summary>
    /// 事件处理接口
    /// </summary>
    public interface IActionHandler<TAction> : IFsmState<TAction>
    {
        /// <summary>
        /// 动作
        /// </summary>
        IAction<TAction> Action { get; }
        /// <summary>
        /// 判断当前状态是否能够执行动作
        /// </summary>
        /// <returns></returns>
        bool CanPerformAction();
        /// <summary>
        /// 添加动作完成回调
        /// </summary>
        /// <param name="onFinishAction"></param>
        void AddFinishCallBack(System.Action onFinishAction);
    }
}
