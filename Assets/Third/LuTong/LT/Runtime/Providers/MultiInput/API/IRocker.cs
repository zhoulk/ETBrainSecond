/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/10
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LT.Net;

namespace LT
{
    /// <summary>
    /// 摇杆消息接口
    /// </summary>
    public interface IRocker : IDevice
    {
        /// <summary>
        /// 向量x值
        /// </summary>
        float X { get; }

        /// <summary>
        /// 向量y值
        /// </summary>
        float Y { get; }

        /// <summary>
        /// 向量
        /// </summary>
        Vector2 Vector { get; }

        /// <summary>
        /// 设置消息
        /// </summary>
        /// <param name="msg"></param>
        void SetMessage(IMessage msg);
    }
}