/*
*    描述:
*          1. 消息管理中心
*
*    开发人: 邓平
*/
using System;
using System.Collections.Generic;

namespace LtFramework.Util
{
    public class MsgRecord
    {
        private static readonly Stack<MsgRecord> _MsgRecordPool = new Stack<MsgRecord>();
        public Enum MsgType;
        public Action<object> OnMsgReceived;

        private MsgRecord()
        {
        }

        public static MsgRecord Allocate(Enum msgType, Action<object> onMsgReceived)
        {
            MsgRecord retRecord = null;
            if (_MsgRecordPool.Count > 0)
            {
                retRecord = _MsgRecordPool.Pop();
            }
            else
            {
                retRecord = new MsgRecord();
            }

            retRecord.MsgType = msgType;
            retRecord.OnMsgReceived = onMsgReceived;
            return retRecord;
        }

        public void Recycle()
        {
            MsgType = null;
            OnMsgReceived = null;
            _MsgRecordPool.Push(this);
        }
    }
}