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
    /// 当应用获取焦点
    /// </summary>
    public interface IOnApplicationFocus
    {
        /// <summary>
        /// Monobehavior OnApplicationFocus
        /// </summary>
        /// <param name="focus">是否获得焦点</param>
        void OnApplicationFocus(bool focus);
    }
}