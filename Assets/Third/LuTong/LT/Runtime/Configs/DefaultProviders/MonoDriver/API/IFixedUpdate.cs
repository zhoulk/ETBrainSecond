/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/4
 * 模块描述： 对应 MonoBehavior 的接口
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.MonoDriver
{
    /// <summary>
    /// FixedUpdate时调用
    /// </summary>
    public interface IFixedUpdate
    {
        /// <summary>
        /// FixedUpdate时调用
        /// </summary>
        void FixedUpdate();
    }
}