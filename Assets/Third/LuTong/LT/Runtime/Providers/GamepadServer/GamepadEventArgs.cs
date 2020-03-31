/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/07/22
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using LT.Net;

namespace LT.GamepadServer
{
    /// <summary>
    /// 手柄消息事件
    /// </summary>
    public class GamepadEventArgs : EventArgs
    {
        /// <summary>
        /// 手柄消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        public GamepadEventArgs(IMessage msg)
        {
            this.Msg = msg;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public IMessage Msg { get; private set; }
    }
}