/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：键值映射表,将Unity的KeyCode映射至自定义KeyCode2
 * 
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    internal partial class MultiInput
    {
        /// <summary>
        /// 键值映射表
        /// KeyCode To KeyCode2
        /// 适配键值，在此处添加
        /// </summary>
        internal class Mapping
        {
            /// <summary>
            /// 1P 按键的映射表,硬件设备，适配键值等皆在此表映射
            /// </summary>
            public static Dictionary<KeyCode, KeyCode2> Table1P = new Dictionary<KeyCode, KeyCode2>()
        {
            { (KeyCode)10,KeyCode2.A},  //小米Tv ok键
            { KeyCode.Joystick1Button0,KeyCode2.A},
            { KeyCode.JoystickButton0,KeyCode2.A},
            { KeyCode.Return,KeyCode2.A},
            { KeyCode.UpArrow,KeyCode2.Up},
            { KeyCode.DownArrow,KeyCode2.Down},
            { KeyCode.LeftArrow,KeyCode2.Left},
            { KeyCode.RightArrow,KeyCode2.Right},
            { KeyCode.Escape,KeyCode2.B},
            { KeyCode.Keypad1,KeyCode2.A},
            { KeyCode.Keypad2,KeyCode2.B},
            { KeyCode.Keypad3,KeyCode2.X},
            { KeyCode.Keypad4,KeyCode2.Y},
            { KeyCode.Keypad7,KeyCode2.L},
            { KeyCode.Keypad8,KeyCode2.R},
            { KeyCode.Alpha1,KeyCode2.X},
            { KeyCode.Alpha2,KeyCode2.Y},
            { KeyCode.Alpha3,KeyCode2.L},
            { KeyCode.Alpha4,KeyCode2.R},
        };

            /// <summary>
            /// 2P按键的映射表
            /// </summary>
            public static Dictionary<KeyCode, KeyCode2> Table2P = new Dictionary<KeyCode, KeyCode2>()
        {
            { KeyCode.A,(KeyCode2)KeyCode2Extend.Left2},
            { KeyCode.D,(KeyCode2)KeyCode2Extend.Right2},
            { KeyCode.W,(KeyCode2)KeyCode2Extend.Up2},
            { KeyCode.S,(KeyCode2)KeyCode2Extend.Down2},
            { KeyCode.J,(KeyCode2)KeyCode2Extend.A2},
            { KeyCode.K,(KeyCode2)KeyCode2Extend.B2},
            { KeyCode.U,(KeyCode2)KeyCode2Extend.X2},
            { KeyCode.I,(KeyCode2)KeyCode2Extend.Y2},
            { KeyCode.Alpha7,(KeyCode2)KeyCode2Extend.L2},
            { KeyCode.Alpha8,(KeyCode2)KeyCode2Extend.R2},
        };

            /// <summary>
            /// 单人模式下，其他P按键映射到1P按键的映射表
            /// </summary>
            public static Dictionary<KeyCode2, KeyCode2> MultiplayerTable = new Dictionary<KeyCode2, KeyCode2>()
        {
            {(KeyCode2)KeyCode2Extend.A2,KeyCode2.A},
            {(KeyCode2)KeyCode2Extend.B2,KeyCode2.B},
            {(KeyCode2)KeyCode2Extend.X2,KeyCode2.X},
            {(KeyCode2)KeyCode2Extend.Y2,KeyCode2.Y},
            {(KeyCode2)KeyCode2Extend.Up2,KeyCode2.Up},
            {(KeyCode2)KeyCode2Extend.Down2,KeyCode2.Down},
            {(KeyCode2)KeyCode2Extend.Left2,KeyCode2.Left},
            {(KeyCode2)KeyCode2Extend.Right2,KeyCode2.Right},
            {(KeyCode2)KeyCode2Extend.L2,KeyCode2.L},
            {(KeyCode2)KeyCode2Extend.R2,KeyCode2.R},
        };

            /// <summary>
            /// 1P映射其他P的映射表，作用在于使用一套键值，响应多套操作
            /// </summary>
            public static Dictionary<int, Dictionary<KeyCode2, KeyCode2>> HidTable = new Dictionary<int, Dictionary<KeyCode2, KeyCode2>>
        {
            { 2,new Dictionary<KeyCode2, KeyCode2>(){
                { KeyCode2.A,(KeyCode2)KeyCode2Extend.A2 },
                { KeyCode2.B,(KeyCode2)KeyCode2Extend.B2 },
                { KeyCode2.X,(KeyCode2)KeyCode2Extend.X2 },
                { KeyCode2.Y,(KeyCode2)KeyCode2Extend.Y2 },
                { KeyCode2.Left,(KeyCode2)KeyCode2Extend.Left2 },
                { KeyCode2.Right,(KeyCode2)KeyCode2Extend.Right2 },
                { KeyCode2.Up,(KeyCode2)KeyCode2Extend.Up2 },
                { KeyCode2.Down,(KeyCode2)KeyCode2Extend.Down2 },
                { KeyCode2.L,(KeyCode2)KeyCode2Extend.L2 },
                { KeyCode2.R,(KeyCode2)KeyCode2Extend.R2 },
            }
            },
        };
        }
    }
}