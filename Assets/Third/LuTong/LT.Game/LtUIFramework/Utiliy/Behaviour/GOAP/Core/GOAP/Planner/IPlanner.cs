using System.Collections.Generic;

namespace DpFrame.GOAP
{
    public interface IPlanner<TAction, TGoal>
    {
        /// <summary>
        /// 计划
        /// </summary>
        Queue<IActionHandler<TAction>> BuildPlan(IGoal<TGoal> goal);
    }
}
