/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/11
 * 模块描述：多人输入模块
 * 
 * 1.支持多人模式
 * 2.支持虚拟手柄
 * 3.支持键盘输入
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using LT.Net;
using LT.EventDispatcher;
using LT.GamepadServer;

namespace LT
{
    /// <summary>
    /// 多人输入实现
    /// </summary>
    internal sealed partial class MultiInput : IInput
    {
        private static KeyCodePool singlePool;
        private static KeyCodePool multiPool;

        private static Dictionary<int, IRocker> Rockers;
        private static Dictionary<int, IGyro> Gyros;
        private static Dictionary<int, IAxis> Axises;

        /// <summary>
        /// 构造方法
        /// </summary>
        public MultiInput(IApplication application)
        {
            App.Make<IEventDispatcher>().AddListener<GamepadEventArgs>(OnGamepadMessage);

            singlePool = new KeyCodePool();
            multiPool = new KeyCodePool();

            Rockers = new Dictionary<int, IRocker>()
            {
                { 0,new Rocker(0)},
                { 1,new Rocker(1)},
                { 2,new Rocker(2)},
            };

            Gyros = new Dictionary<int, IGyro>()
            {
                { 0,new Gyro(0)},
                { 1,new Gyro(1)},
                { 2,new Gyro(2)},
            };

            Axises = new Dictionary<int, IAxis>()
            {
                { 0,new Axis(0)},
                { 1,new Axis(1)},
                { 2,new Axis(2)},
            };
        }

        /// <inheritdoc />
        public void Update()
        {
            //执行键盘的1P键值映射
            foreach (var item in Mapping.Table1P)
            {
                if (UnityEngine.Input.GetKeyDown(item.Key))
                {
                    singlePool.CacheKeyDown(item.Value);
                    multiPool.CacheKeyDown(item.Value);
                }

                if (UnityEngine.Input.GetKeyUp(item.Key))
                {
                    singlePool.CacheKeyUp(item.Value);
                    multiPool.CacheKeyUp(item.Value);
                }
            }

            //执行键盘的2P键值映射
            foreach (var item in Mapping.Table2P)
            {
                if (UnityEngine.Input.GetKeyDown(item.Key))
                {
                    singlePool.CacheKeyDown(item.Value);
                    multiPool.CacheKeyDown(item.Value);
                }

                if (UnityEngine.Input.GetKeyUp(item.Key))
                {
                    singlePool.CacheKeyUp(item.Value);
                    multiPool.CacheKeyUp(item.Value);
                }
            }

            // 将其他键值最终映射成1P键值
            foreach (var item in Mapping.MultiplayerTable)
            {
                if (GetKeyDown(item.Key))
                    singlePool.CacheKeyDown(item.Value);

                if (GetKeyUp(item.Key))
                    singlePool.CacheKeyUp(item.Value);
            }

            //更新轴向
            foreach (var a in Axises.Values)
            {
                a.Update();
            }
        }

        /// <summary>
        /// 固定更新
        /// </summary>
        public void FixedUpdate() { }

        /// <summary>
        /// 延迟更新
        /// </summary>
        public void LateUpdate()
        {
            singlePool.EndOfFrame();
            multiPool.EndOfFrame();
        }

        /// <summary>
        /// 输入手柄消息
        /// </summary>
        /// <param name="msg">消息</param>
        private void OnGamepadMessage(GamepadEventArgs args)
        {
            switch (args.Msg.GetMessageType())
            {
                case MessageType.Keyboard:
                    MessageKeyboard kb = args.Msg as MessageKeyboard;

                    //Debug.Log($"Keyboard  Hid:{kb.Hid} KeyCode:{kb.KeyCode} State:{kb.State}");
                    singlePool.Cache(kb.KeyCode, kb.State);
                    multiPool.Cache(kb.KeyCode, kb.State);
                    break;

                case MessageType.Rocker:
                    MessageRocker r = args.Msg as MessageRocker;
                    if (Rockers.TryGetValue(r.Hid, out IRocker rocker))
                    {
                        rocker.SetMessage(args.Msg);
                    }

                    //Debug.Log($"Rocker  Hid:{r.Hid} KeyCode:{r.KeyCode} State:{r.State}");

                    singlePool.Cache(r.KeyCode, r.State);
                    multiPool.Cache(r.KeyCode, r.State);
                    break;

                case MessageType.Gyro:
                    MessageGyro g = args.Msg as MessageGyro;
                    if (Gyros.TryGetValue(g.Hid, out IGyro gyro))
                    {
                        gyro.SetMessage(args.Msg);
                    }
                    break;
            }
        }

        /// <summary>
        /// 检测任意键
        /// </summary>
        public bool AnyKey
        {
            get { return singlePool.KeyDown.Count > 0 || multiPool.KeyDown.Count > 0; }
        }

        /// <summary>
        /// 检测按键按下
        /// </summary>
        public bool GetKeyDown(KeyCode2 kc)
        {
            return singlePool.KeyDown.Contains(kc);
        }

        /// <summary>
        /// 检测按键持续按下
        /// </summary>
        public bool GetKey(KeyCode2 kc)
        {
            return singlePool.KeyPress.Contains(kc);
        }

        /// <summary>
        /// 检测按键弹起
        /// </summary>
        public bool GetKeyUp(KeyCode2 kc)
        {
            return singlePool.KeyUp.Contains(kc);
        }

        /// <summary>
        /// 依据手柄hid，检测按键弹起
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns> 按下返回true </returns>
        public bool GetKeyDown(KeyCode2 kc, int hid)
        {
            return CheckContains(multiPool.KeyDown, kc, hid);
        }

        /// <summary>
        /// 依据手柄hid，检测按键持续按下
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns>按下返回true</returns>
        public bool GetKey(KeyCode2 kc, int hid)
        {
            return CheckContains(multiPool.KeyPress, kc, hid);
        }

        /// <summary>
        /// 依据手柄hid，检测按键弹起
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="hid"> 手柄id </param>
        /// <returns>弹起返回true</returns>
        public bool GetKeyUp(KeyCode2 kc, int hid)
        {
            return CheckContains(multiPool.KeyUp, kc, hid);
        }

        /// <summary>
        /// 获取摇杆
        /// </summary>
        /// <param name="hid">手柄id</param>
        /// <returns>返回摇杆接口</returns>
        public IRocker GetRocker(int hid)
        {
            return Rockers[hid];
        }

        /// <summary>
        /// 获取陀螺仪
        /// </summary>
        /// <param name="hid"> 手柄id </param>
        /// <returns>返回陀螺仪接口</returns>
        public IGyro GetGyro(int hid = 1)
        {
            return Gyros[hid];
        }

        /// <summary>
        /// 获取轴向信息
        /// </summary>
        /// <returns></returns>
        public IAxis GetAxis()
        {
            return Axises[0];
        }

        /// <summary>
        /// 获取轴向信息
        /// </summary>
        /// <param name="hid"> 手柄id </param>
        /// <returns>返回轴向接口</returns>
        public IAxis GetAxis(int hid)
        {
            return Axises[hid];
        }

        #region 私有方法
        /// <summary>
        /// 检测列表包含内容
        /// </summary>
        /// <param name="target">目标列表</param>
        /// <param name="kc">键值</param>
        /// <param name="hid">手柄id</param>
        private bool CheckContains(ISet<KeyCode2> target, KeyCode2 kc, int hid)
        {
            if (hid == 1)
            {
                return target.Contains(kc);
            }
            else if (Mapping.HidTable.TryGetValue(hid, out Dictionary<KeyCode2, KeyCode2> hidTable) && hidTable.TryGetValue(kc, out KeyCode2 realkc))
            {
                return target.Contains(realkc);
            }
            return false;
        }
        #endregion
    }

    /// <summary>
    /// 键值池
    /// </summary>
    internal class KeyCodePool
    {
        public HashSet<KeyCode2> KeyDown = new HashSet<KeyCode2>();
        public HashSet<KeyCode2> KeyPress = new HashSet<KeyCode2>();
        public HashSet<KeyCode2> KeyUp = new HashSet<KeyCode2>();

        /// <summary>
        /// 键值入池
        /// </summary>
        /// <param name="kc">键值</param>
        /// <param name="state">键值状态</param>
        public void Cache(KeyCode2 kc, KeyboardState state)
        {
            if (state == KeyboardState.KeyDown)
            {
                CacheKeyDown(kc);
            }
            else if (state == KeyboardState.KeyUp)
            {
                CacheKeyUp(kc);
            }
        }

        /// <summary>
        /// 缓存KeyDown池
        /// </summary>
        /// <param name="kc">Kc.</param>
        public void CacheKeyDown(KeyCode2 kc)
        {
            KeyDown.Add(kc);
            KeyPress.Add(kc);
        }

        /// <summary>
        /// 缓存KeyUp池
        /// </summary>
        /// <param name="kc">Kc.</param>
        public void CacheKeyUp(KeyCode2 kc)
        {
            KeyUp.Add(kc);
            if (KeyPress.Contains(kc)) KeyPress.Remove(kc);
        }

        /// <summary>
        /// 每帧结束
        /// </summary>
        public void EndOfFrame()
        {
            KeyDown.Clear();
            KeyUp.Clear();
        }
    }
}