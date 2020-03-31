/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/11/20
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.Fsm
{
    /// <summary>
    /// 有限状态机任意状态
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public abstract class AnyState<T> : FsmState<T> where T : class
    {
        /// <summary>
        /// 是否为任意状态
        /// </summary>
        public override bool IsAnyState => true;
    }
}