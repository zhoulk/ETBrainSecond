/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/07/03
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.MonoDriver
{
    /// <summary>
    /// 当应用暂停
    /// </summary>
    public interface IOnApplicationPause
    {
        /// <summary>
        /// Monobehavior OnApplicationPause
        /// </summary>
        /// <param name="pause">是否暂停</param>
        void OnApplicationPause(bool pause);
    }
}