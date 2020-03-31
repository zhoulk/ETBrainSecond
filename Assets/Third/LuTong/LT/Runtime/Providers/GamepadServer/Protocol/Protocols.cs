/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：
 * 1. 协议集合
 * 2. 每个新协议的实现，需要在Encode最后，调用UpdateLength方法更新长度信息
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LT.Net
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        /// 数据消息
        /// </summary>
        Msg = 0,

        /// <summary>
        /// 心跳消息
        /// </summary>
        Heard = 1,

        /// <summary>
        /// 键盘
        /// </summary>
        Keyboard = 2,

        /// <summary>
        /// 摇杆
        /// </summary>
        Rocker = 3,

        /// <summary>
        /// 陀螺仪
        /// </summary>
        Gyro = 4,

        /// <summary>
        /// 修改样式
        /// </summary>
        ChangeStyle = 100,

        /// <summary>
        /// 连接/断开消息
        /// </summary>
        Connect = 101,
    }

    /// <summary>
    /// 协议头
    /// </summary>
    public class MessageHeader : IMessage
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public byte Version = 0x01;

        /// <summary>
        /// 消息类型，0 心跳消息
        /// </summary>
        public MessageType MsgType;

        /// <summary>
        /// 包序号
        /// </summary>
        public ushort TimeStamp;

        /// <summary>
        /// 设备ID，1P = 1，2P = 2
        /// </summary>
        public byte Hid;

        /// <summary>
        /// 包体长度
        /// </summary>
        public ushort Length;

        /// <summary>
        /// 标记包体长度的起始位，用于更新包体长度信息
        /// </summary>
        private int lengthStartIndex;

        /// <summary>
        /// 序列缓存
        /// </summary>
        protected List<byte> buf;

        public MessageHeader()
        {
            buf = new List<byte>(36);
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public void Clear()
        {
            buf.Clear();
        }

        /// <summary>
        /// 转化为二进制
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Encode()
        {
            buf.Add(Version);
            buf.Add((byte)MsgType);
            buf.AddRange(BitConverter.GetBytes(TimeStamp));
            buf.Add(Hid);

            lengthStartIndex = 5;

            buf.AddRange(BitConverter.GetBytes(Length));

            return buf.ToArray();
        }

        /// <summary>
        /// 更新包头长度信息
        /// </summary>
        public void Refresh()
        {
            //刷新包长信息
            Length = (ushort)buf.Count;
            byte[] date = BitConverter.GetBytes(Length);
            for (int i = 0; i < date.Length; i++)
            {
                buf[lengthStartIndex] = date[i];
                lengthStartIndex++;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="buf"></param>
        public virtual int Decode(byte[] buf, int startIndex)
        {
            Version = buf[startIndex];
            startIndex += 1;
            MsgType = (MessageType)buf[startIndex];
            startIndex += 1;
            TimeStamp = BitConverter.ToUInt16(buf, startIndex);
            startIndex += 2;
            Hid = buf[startIndex];
            startIndex += 1;
            Length = BitConverter.ToUInt16(buf, startIndex);
            startIndex += 2;

            return startIndex;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public string ConvertString()
        {
            byte[] bytes = buf.ToArray();
            string data = System.Text.Encoding.Default.GetString(bytes);
            return data;
        }

        public MessageType GetMessageType()
        {
            return MsgType;
        }
    }

    /// <summary>
    /// 心跳消息
    /// </summary>
    public class MessageHeart : MessageHeader
    {
        public MessageHeart() : base()
        {
            MsgType = MessageType.Heard;
        }

        public override byte[] Encode()
        {
            base.Encode();

            //更新长度信息
            Refresh();

            return buf.ToArray();
        }
    }

    /// <summary>
    /// 键盘消息
    /// </summary>
    public class MessageKeyboard : MessageHeader
    {
        /// <summary>
        /// 键值
        /// </summary>
        public KeyCode2 KeyCode;

        public KeyboardState State;

        public MessageKeyboard() : base()
        {
            MsgType = MessageType.Keyboard;
        }

        public override byte[] Encode()
        {
            base.Encode();

            buf.AddRange(BitConverter.GetBytes((UInt16)KeyCode));
            buf.Add((byte)State);

            //更新长度信息
            Refresh();

            return buf.ToArray();
        }

        public override int Decode(byte[] buf, int startIndex)
        {
            startIndex = base.Decode(buf, startIndex);

            KeyCode = (KeyCode2)BitConverter.ToUInt16(buf, startIndex);
            startIndex += 2;
            State = (KeyboardState)buf[startIndex];
            startIndex += 1;

            return startIndex;
        }
    }

    /// <summary>
    /// 摇杆消息
    /// </summary>
    public class MessageRocker : MessageHeader
    {
        public byte Rid;
        public float Rx;
        public float Ry;

        /// <summary>
        /// 键值
        /// </summary>
        public KeyCode2 KeyCode;

        public KeyboardState State;

        public MessageRocker() : base()
        {
            MsgType = MessageType.Rocker;
        }

        public override byte[] Encode()
        {
            base.Encode();

            byte[] rx = BitConverter.GetBytes(Rx);
            byte[] ry = BitConverter.GetBytes(Ry);
            byte[] kc = BitConverter.GetBytes((UInt16)KeyCode);

            buf.Add(Rid);
            buf.AddRange(rx);
            buf.AddRange(ry);
            buf.AddRange(kc);
            buf.Add((byte)State);

            //更新长度信息
            Refresh();

            return buf.ToArray();
        }

        public override int Decode(byte[] buf, int startIndex)
        {
            startIndex = base.Decode(buf, startIndex);

            Rid = buf[startIndex];
            startIndex += 1;
            Rx = BitConverter.ToSingle(buf, startIndex);
            startIndex += 4;
            Ry = BitConverter.ToSingle(buf, startIndex);
            startIndex += 4;
            KeyCode = (KeyCode2)BitConverter.ToUInt16(buf, startIndex);
            startIndex += 2;
            State = (KeyboardState)buf[startIndex];
            startIndex += 1;

            return startIndex;
        }
    }

    /// <summary>
    /// 陀螺仪消息
    /// </summary>
    public class MessageGyro : MessageHeader
    {
        /// <summary>
        /// 重力加速度
        /// </summary>
        public Vector3 Gravity;

        /// <summary>
        /// 无重力的加速度
        /// </summary>
        public Vector3 UserAcceleration;

        /// <summary>
        /// 旋转速度
        /// </summary>
        public Vector3 RotationRate;

        /// <summary>
        /// 四元素
        /// </summary>
        public Quaternion Attitude;

        public MessageGyro() : base()
        {
            MsgType = MessageType.Gyro;
        }

        public override byte[] Encode()
        {
            base.Encode();

            byte[] gx = BitConverter.GetBytes(Gravity.x);
            byte[] gy = BitConverter.GetBytes(Gravity.y);
            byte[] gz = BitConverter.GetBytes(Gravity.z);

            byte[] ux = BitConverter.GetBytes(UserAcceleration.x);
            byte[] uy = BitConverter.GetBytes(UserAcceleration.y);
            byte[] uz = BitConverter.GetBytes(UserAcceleration.z);

            byte[] rx = BitConverter.GetBytes(RotationRate.x);
            byte[] ry = BitConverter.GetBytes(RotationRate.y);
            byte[] rz = BitConverter.GetBytes(RotationRate.z);

            byte[] ax = BitConverter.GetBytes(Attitude.x);
            byte[] ay = BitConverter.GetBytes(Attitude.y);
            byte[] az = BitConverter.GetBytes(Attitude.z);
            byte[] aw = BitConverter.GetBytes(Attitude.w);

            buf.AddRange(gx);
            buf.AddRange(gy);
            buf.AddRange(gz);

            buf.AddRange(ux);
            buf.AddRange(uy);
            buf.AddRange(uz);

            buf.AddRange(rx);
            buf.AddRange(ry);
            buf.AddRange(rz);

            buf.AddRange(ax);
            buf.AddRange(ay);
            buf.AddRange(az);
            buf.AddRange(aw);

            //更新长度信息
            Refresh();

            return buf.ToArray();
        }

        public override int Decode(byte[] value, int startIndex)
        {
            startIndex = base.Decode(value, startIndex);

            Gravity.x = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            Gravity.y = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            Gravity.z = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;

            UserAcceleration.x = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            UserAcceleration.y = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            UserAcceleration.z = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;

            RotationRate.x = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            RotationRate.y = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            RotationRate.z = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;

            Attitude.x = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            Attitude.y = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            Attitude.z = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;
            Attitude.w = BitConverter.ToSingle(value, startIndex);
            startIndex += 4;

            return startIndex;
        }
    }

    /// <summary>
    /// 请求连接/断开消息
    /// </summary>
    public class MessageConnect : MessageHeader
    {
        /// <summary>
        /// 1 连接，2断开
        /// </summary>
        public byte State;

        public MessageConnect() : base()
        {
            MsgType = MessageType.Connect;
        }

        public override byte[] Encode()
        {
            base.Encode();

            buf.Add(State);

            //更新长度信息
            Refresh();

            return buf.ToArray();
        }

        public override int Decode(byte[] value, int startIndex)
        {
            startIndex = base.Decode(value, startIndex);
            State = value[startIndex];
            startIndex++;

            return startIndex;
        }
    }

    /// <summary>
    /// 消息协议
    /// </summary>
    public class MessageMsg : MessageHeader
    {
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data;

        public MessageMsg() : base()
        {
            MsgType = MessageType.Msg;
        }

        public override byte[] Encode()
        {
            base.Encode();

            buf.AddRange(Data);

            //更新长度信息
            Refresh();

            return buf.ToArray();
        }

        public override int Decode(byte[] value, int startIndex)
        {
            startIndex = base.Decode(value, startIndex);
            int size = value.Length - startIndex;
            Data = new byte[size];
            Array.Copy(value, startIndex, Data, 0, size);
            return startIndex;
        }
    }
}