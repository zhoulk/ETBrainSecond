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
    /// 当被释放时
    /// </summary>
    public interface IOnDestroy
    {
        /// <summary>
        /// Monobehavior OnDestroy
        /// </summary>
        void OnDestroy();
    }
}