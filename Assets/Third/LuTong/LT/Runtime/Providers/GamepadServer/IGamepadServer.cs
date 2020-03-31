/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：手柄服务器接口
 * 
 * ------------------------------------------------------------------------------*/

using LT.MonoDriver;

namespace LT.GamepadServer
{
    /// <summary>
    /// 手柄Server接口
    /// </summary>
    public interface IGamepadServer
    {
        /// <summary>
        /// 广播数据到所有已连接手柄
        /// </summary>
        /// <param name="bytes">数据</param>
        void BroadcastToGamepad(byte[] bytes);
    }
}