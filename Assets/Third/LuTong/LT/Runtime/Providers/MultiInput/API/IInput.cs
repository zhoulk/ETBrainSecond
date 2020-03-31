/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：输入接口
 * 
 * ------------------------------------------------------------------------------*/
using LT.Net;
using LT.MonoDriver;

namespace LT
{
    /// <summary>
    /// 输入接口
    /// </summary>
    public interface IInput : IUpdate, ILateUpdate, IFixedUpdate
    {
        /// <summary>
        /// 任意键按下
        /// </summary>
        bool AnyKey { get; }

        /// <summary>
        /// 检测按键按下
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <returns></returns>
        bool GetKeyDown(KeyCode2 kc);

        /// <summary>
        /// 检测按键持续按下
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <returns></returns>
        bool GetKey(KeyCode2 kc);

        /// <summary>
        /// 检测按键弹起
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <returns></returns>
        bool GetKeyUp(KeyCode2 kc);

        /// <summary>
        /// 依据手柄hid，检测按键按下
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        bool GetKeyDown(KeyCode2 kc, int hid);

        /// <summary>
        /// 依据手柄hid，检测按键持续按下
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        bool GetKey(KeyCode2 kc, int hid);

        /// <summary>
        /// 依据手柄hid，检测按键弹起
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        bool GetKeyUp(KeyCode2 kc, int hid);

        /// <summary>
        /// 获取轴向信息
        /// </summary>
        /// <returns></returns>
        IAxis GetAxis();

        /// <summary>
        /// 依据手柄hid，获取轴向信息
        /// </summary>
        /// <param name="hid">手柄id</param>
        /// <returns></returns>
        IAxis GetAxis(int hid = 1);

        /// <summary>
        /// 依据手柄hid，获取摇杆信息
        /// </summary>
        /// <param name="hid">手柄id</param>
        /// <returns></returns>
        IRocker GetRocker(int hid = 1);

        /// <summary>
        /// 依据手柄hid，获取陀螺仪信息
        /// </summary>
        /// <param name="hid">手柄id</param>
        /// <returns></returns>
        IGyro GetGyro(int hid = 1);
    }
}