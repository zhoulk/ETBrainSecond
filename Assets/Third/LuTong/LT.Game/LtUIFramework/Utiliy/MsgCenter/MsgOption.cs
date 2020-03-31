/*
*    描述:
*          1. 消息管理
*
*    开发人: 邓平
*/
using System;
using System.Collections.Generic;

namespace LtFramework.Util
{

    public class MsgOption
    {
        //存储当前模块注册的消息  当模块销毁时注销
        List<MsgRecord> mMsgRecords = new List<MsgRecord>();

        /// <summary>
        /// 消息注册
        /// </summary>
        /// <param UIName="msgType"></param>
        /// <param UIName="msgListener"></param>
        public void RegisterMsg(Enum msgType, Action<object> msgListener)
        {
            MsgCenter.RegisterMsg(msgType, msgListener);
            mMsgRecords.Add(MsgRecord.Allocate(msgType, msgListener));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param UIName="msgType"></param>
        /// <param UIName="data"></param>
        public void SendMsg(Enum msgType, object data = null)
        {
            MsgCenter.SendMsg(msgType, data);
        }


        /// <summary>
        /// 注销指定消息
        /// </summary>
        /// <param UIName="msgType"></param>
        public void UnRegisterMsg(Enum msgType)
        {
            var selectedRecords = mMsgRecords.FindAll((record) => Equals(record.MsgType, msgType));

            selectedRecords.ForEach(record =>
            {
                MsgCenter.UnRegister(record.MsgType, record.OnMsgReceived);
                mMsgRecords.Remove(record);

                record.Recycle();
            });

            selectedRecords.Clear();
        }

        /// <summary>
        /// 注销指定消息
        /// </summary>
        /// <param UIName="msgType"></param>
        /// <param UIName="onMsgReceived"></param>
        public void UnRegisterMsg(Enum msgType, Action<object> onMsgReceived)
        {
            var selectedRecords =
                mMsgRecords.FindAll(
                    record => Equals(record.MsgType, msgType) && Equals(record.OnMsgReceived, onMsgReceived));

            selectedRecords.ForEach(record =>
            {
                MsgCenter.UnRegister(record.MsgType, record.OnMsgReceived);
                mMsgRecords.Remove(record);

                record.Recycle();
            });

            selectedRecords.Clear();
        }

        /// <summary>
        /// 注销所有消息
        /// </summary>
        public void UnRegiserAllMsg()
        {
            foreach (MsgRecord mMsgRecord in mMsgRecords)
            {
                MsgCenter.UnRegister(mMsgRecord.MsgType, mMsgRecord.OnMsgReceived);
                mMsgRecord.Recycle();
            }
        }
    }
}
