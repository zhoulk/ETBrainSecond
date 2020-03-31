/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：轴向信息类
 * 
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using LT;

namespace LT
{
    internal partial class MultiInput
    {
        /// <summary>
        /// 轴向
        /// </summary>
        internal class Axis : IAxis
        {
            private int hid;

            private float vertical;
            private float horizontal;

            private int verticalRaw;
            private int horizontalRaw;

            private float vTarget;
            private float hTarget;

            private float vPercent;
            private float hPercent;

            private int vFactor;
            private int hFactor;

            private Dictionary<KeyCode2, int> kc2Value;
            private Dictionary<KeyCode2, int> targetArray;

            public Axis(byte devID)
            {
                this.hid = devID;

                kc2Value = new Dictionary<KeyCode2, int>()
            {
                { KeyCode2.Up,1},
                { KeyCode2.Down,-1},
                { KeyCode2.Left,-1},
                { KeyCode2.Right,1},
            };

                targetArray = new Dictionary<KeyCode2, int>()
            {
                { KeyCode2.Up,0},
                { KeyCode2.Down,0},
                { KeyCode2.Left,0},
                { KeyCode2.Right,0},
            };
            }

            public void Update()
            {
                //更新按键状态
                foreach (var k in kc2Value)
                {
                    if (hid == 0)
                    {
                        if (LTInput.GetKeyDown(k.Key))
                        {
                            UpdateKeyDown(k.Key, k.Value);
                        }

                        if (LTInput.GetKeyUp(k.Key))
                        {
                            UpdateKeyUp(k.Key, 0);
                        }
                    }
                    else
                    {
                        if (LTInput.GetKeyDown(k.Key, hid))
                        {
                            UpdateKeyDown(k.Key, k.Value);
                        }

                        if (LTInput.GetKeyUp(k.Key, hid))
                        {
                            UpdateKeyUp(k.Key, 0);
                        }
                    }
                }

                //更新插值逻辑
                UpdateLerp();
            }

            private void UpdateKeyDown(KeyCode2 kc, int value)
            {
                targetArray[kc] = value;

                if (kc == KeyCode2.Left || kc == KeyCode2.Right)
                {
                    horizontalRaw = targetArray[kc];

                    //目标不同，则进度归 0
                    if (hTarget != targetArray[kc])
                        hPercent = 0f;

                    hTarget = targetArray[kc];
                    hFactor = 1;
                }
                else if (kc == KeyCode2.Up || kc == KeyCode2.Down)
                {
                    verticalRaw = targetArray[kc];

                    //目标不同，则进度归 0
                    if (vTarget != targetArray[kc])
                        vPercent = 0f;

                    vTarget = targetArray[kc];
                    vFactor = 1;
                }
            }

            private void UpdateKeyUp(KeyCode2 kc, int value)
            {
                targetArray[kc] = value;

                if (kc == KeyCode2.Left)
                {
                    horizontalRaw = targetArray[KeyCode2.Right];

                    if (targetArray[KeyCode2.Right] == 0)
                    {
                        hFactor = -1;
                    }
                    else
                    {
                        hTarget = targetArray[KeyCode2.Right];
                        hPercent = 0f;
                    }
                }
                else if (kc == KeyCode2.Right)
                {
                    horizontalRaw = targetArray[KeyCode2.Left];

                    if (targetArray[KeyCode2.Left] == 0)
                    {
                        hFactor = -1;
                    }
                    else
                    {
                        hTarget = targetArray[KeyCode2.Left];
                        hPercent = 0f;
                    }
                }
                else if (kc == KeyCode2.Up)
                {
                    verticalRaw = targetArray[KeyCode2.Down];

                    if (targetArray[KeyCode2.Down] == 0)
                    {
                        vFactor = -1;
                    }
                    else
                    {
                        vTarget = targetArray[KeyCode2.Down];
                        vPercent = 0f;
                    }
                }
                else if (kc == KeyCode2.Down)
                {
                    verticalRaw = targetArray[KeyCode2.Up];

                    if (targetArray[KeyCode2.Up] == 0)
                    {
                        vFactor = -1;
                    }
                    else
                    {
                        vTarget = targetArray[KeyCode2.Up];
                        vPercent = 0f;
                    }
                }
            }

            private void UpdateLerp()
            {
                vertical = Mathf.Lerp(0, vTarget, vPercent);
                horizontal = Mathf.Lerp(0, hTarget, hPercent);

                vPercent += Time.deltaTime * vFactor * 3f;
                hPercent += Time.deltaTime * hFactor * 3f;

                vPercent = Mathf.Clamp(vPercent, 0f, 1f);
                hPercent = Mathf.Clamp(hPercent, 0f, 1f);
            }


            public int Hid
            {
                get { return hid; }
            }

            public float Vertical
            {
                get { return vertical; }
            }

            public float Horizontal
            {
                get { return horizontal; }
            }

            public int VerticalRaw
            {
                get { return verticalRaw; }
            }

            public int HorizontalRaw
            {
                get { return horizontalRaw; }
            }
        }
    }
}