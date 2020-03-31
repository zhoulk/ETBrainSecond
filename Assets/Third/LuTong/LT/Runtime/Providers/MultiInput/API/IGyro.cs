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
    /// 陀螺仪接口
    /// </summary>
    public interface IGyro : IDevice
    {
        /// <summary>
        /// 重力加速度
        /// </summary>
        Vector3 Gravity { get; }

        /// <summary>
        /// 无重力的加速度
        /// </summary>
        Vector3 UserAcceleration { get; }

        /// <summary>
        /// 旋转速度
        /// </summary>
        Vector3 RotationRate { get; }

        /// <summary>
        /// 四元素
        /// </summary>
        Quaternion Attitude { get; }

        /// <summary>
        /// 设置消息
        /// </summary>
        /// <param name="msg"></param>
        void SetMessage(IMessage msg);
    }
}

