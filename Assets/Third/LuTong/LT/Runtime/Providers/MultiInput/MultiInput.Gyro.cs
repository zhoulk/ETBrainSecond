/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：陀螺仪信息类
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LT.Net;

namespace LT
{
    internal partial class MultiInput
    {
        /// <summary>
        /// 陀螺仪
        /// </summary>
        internal class Gyro : IGyro
        {
            private int hid;
            private Vector3 g;
            private Vector3 u;
            private Vector3 r;
            private Quaternion a;

            public Gyro(byte id)
            {
                this.hid = id;
            }

            public int Hid
            {
                get { return hid; }
            }

            public void SetMessage(IMessage msg)
            {
                MessageGyro m = msg as MessageGyro;

                g = m.Gravity;
                u = m.UserAcceleration;
                r = m.RotationRate;
                a = m.Attitude;
            }

            public virtual void Update() { }

            public Vector3 Gravity
            {
                get { return g; }
            }

            public Vector3 UserAcceleration
            {
                get { return u; }
            }

            public Vector3 RotationRate
            {
                get { return r; }
            }

            public Quaternion Attitude
            {
                get { return a; }
            }
        }
    }
}
