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
    /// 当应用退出时
    /// </summary>
    public interface IOnApplicationQuit
    {
        /// <summary>
        /// Monobehavior OnApplicationQuit
        /// </summary>
        void OnApplicationQuit();
    }
}