/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/10
 * 模块描述：路通Input外观
 * 
 * ------------------------------------------------------------------------------*/

using LT.GamepadServer;

namespace LT
{
    /// <summary>
    /// MultiInput门面。
    /// <para>1.支持多人模式</para>
    /// <para>2.支持虚拟手柄，需开启<see cref="ProviderTestGamepadServer"/> </para>
    /// <para>3.支持Keyboard</para>
    /// </summary>
    public sealed class LTInput : Facade<IInput>
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <returns>如果可用，返回true</returns>
        public static bool IsUsable()
        {
            return That != null;
        }

        /// <summary>
        /// 检测任意键按下
        /// </summary>
        public static bool AnyKey
        {
            get { return That.AnyKey; }
        }

        /// <summary>
        /// 检测按键按下
        /// </summary>
        public static bool GetKeyDown(KeyCode2 kc)
        {
            return That.GetKeyDown(kc);
        }

        /// <summary>
        /// 检测按键持续按下
        /// </summary>
        public static bool GetKey(KeyCode2 kc)
        {
            return That.GetKey(kc);
        }

        /// <summary>
        /// 检测按键弹起
        /// </summary>
        public static bool GetKeyUp(KeyCode2 kc)
        {
            return That.GetKeyUp(kc);
        }

        /// <summary>
        /// 依据手柄hid，检测按键弹起
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        public static bool GetKeyDown(KeyCode2 kc, int hid)
        {
            return That.GetKeyDown(kc, hid);
        }

        /// <summary>
        /// 依据手柄hid，检测按键持续按下
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        public static bool GetKey(KeyCode2 kc, int hid)
        {
            return That.GetKey(kc, hid);
        }

        /// <summary>
        /// 依据手柄hid，检测按键弹起
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        public static bool GetKeyUp(KeyCode2 kc, int hid)
        {
            return That.GetKeyUp(kc, hid);
        }

        /// <summary>
        /// 获取摇杆
        /// </summary>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        public static IRocker GetRocker(int hid)
        {
            return That.GetRocker(hid);
        }

        /// <summary>
        /// 获取陀螺仪
        /// </summary>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        public static IGyro GetGyro(int hid)
        {
            return That.GetGyro(hid);
        }

        /// <summary>
        /// 获取轴向信息
        /// </summary>
        /// <returns></returns>
        public static IAxis GetAxis()
        {
            return That.GetAxis();
        }

        /// <summary>
        /// 依据手柄id，获取轴向信息
        /// </summary>
        /// <param name="hid"> 手柄id </param>
        /// <returns></returns>
        public static IAxis GetAxis(int hid)
        {
            return That.GetAxis(hid);
        }
    }
}