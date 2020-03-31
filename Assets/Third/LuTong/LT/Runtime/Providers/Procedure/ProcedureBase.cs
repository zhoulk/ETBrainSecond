/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT.Fsm;

namespace LT.Procedure
{
    /// <summary>
    /// 流程基类。
    /// </summary>
    public abstract class ProcedureBase : FsmState<IProcedureManager>
    {
        /// <summary>
        /// 状态初始化时调用。
        /// </summary>
        /// <param name="fsm">流程状态机。</param>
        public override void OnInit()
        {
            base.OnInit();
        }

        /// <summary>
        /// 进入流程时调用。
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();
        }

        /// <summary>
        /// 离开流程时调用。
        /// </summary>
        /// <param name="isShutdown">是否是关闭状态机时触发。</param>
        public override void OnLeave(bool isShutdown)
        {
            base.OnLeave(isShutdown);
        }

        /// <summary>
        /// 状态轮询时调用。
        /// </summary>
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        /// <summary>
        /// 流程销毁时调用。
        /// </summary>
        /// <param name="fsm">流程状态机。</param>
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}