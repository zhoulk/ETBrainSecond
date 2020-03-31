/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：摇杆信息类
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LT.Net;

namespace LT
{
    internal partial class MultiInput
    {
        /// <summary>
        /// 摇杆信息类
        /// </summary>
        internal class Rocker : IRocker
        {
            private int hid;
            private Vector2 vector;

            public Rocker(byte id)
            {
                this.hid = id;
            }

            public void SetMessage(IMessage msg)
            {
                MessageRocker m = msg as MessageRocker;

                vector.x = m.Rx;
                vector.y = m.Ry;
            }

            public virtual void Update() { }

            public int Hid
            {
                get { return hid; }
            }

            public float X
            {
                get { return vector.x; }
            }

            public float Y
            {
                get { return vector.y; }
            }

            public Vector2 Vector
            {
                get { return vector; }
            }
        }
    }
}