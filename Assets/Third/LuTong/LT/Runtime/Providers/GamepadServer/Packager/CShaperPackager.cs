/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：C# 手柄测试Server 的解包器
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT.Net
{
    public class CShaperPackager : IPackager
    {
        MessageHeader header;
        MessageKeyboard keyboard;
        MessageRocker rocker;
        MessageGyro gyro;
        MessageHeart heart;
        MessageConnect connect;
        MessageMsg msg;

        public CShaperPackager()
        {
            header = new MessageHeader();
            keyboard = new MessageKeyboard();
            rocker = new MessageRocker();
            gyro = new MessageGyro();
            heart = new MessageHeart();
            connect = new MessageConnect();
            msg = new MessageMsg();
        }

        public virtual IMessage Decode(byte[] bytes)
        {
            header.Clear();
            header.Decode(bytes, 0);

            switch (header.GetMessageType())
            {
                case MessageType.Keyboard:
                    keyboard.Clear();
                    keyboard.Decode(bytes, 0);
                    return keyboard;

                case MessageType.Rocker:
                    rocker.Clear();
                    rocker.Decode(bytes, 0);
                    return rocker;

                case MessageType.Gyro:
                    gyro.Clear();
                    gyro.Decode(bytes, 0);
                    return gyro;

                case MessageType.Heard:
                    heart.Clear();
                    heart.Decode(bytes, 0);
                    return heart;

                case MessageType.Connect:
                    connect.Clear();
                    connect.Decode(bytes, 0);
                    return connect;

                case MessageType.Msg:
                    msg.Clear();
                    msg.Decode(bytes, 0);
                    return msg;
            }

            return null;
        }

        public virtual IMessage Decode(string data) { return null; }
    }

}
