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
    public class MsgCenter
    {
        #region 注册事件 全局消息调用

        private static Dictionary<Enum, Action<object>> mRegisteredMsgs = new Dictionary<Enum, Action<object>>();

        /// <summary>
        /// 注册 消息
        /// </summary>
        /// <param UIName="msgEnum"> 消息体 </param>
        /// <param UIName="onMsgReceived"> 执行委托 </param>
        public static void RegisterMsg(Enum msgType, Action<object> onMsgReceived)
        {
            if (!mRegisteredMsgs.ContainsKey(msgType))
            {
                mRegisteredMsgs.Add(msgType, p => { });
            }
            mRegisteredMsgs[msgType] += onMsgReceived;
            //Debug.Log("注册消息" + msgType + "   number :" + MsgCenter.GetMsgCount(msgType));
        }

        public static int GetMsgCount(Enum msgType)
        {
            if (mRegisteredMsgs.ContainsKey(msgType))
            {
                var array = mRegisteredMsgs[msgType].GetInvocationList();
                if (array.Length > 1)
                {
                    return array.Length -1;
                }

            }
            return 0;
        }

        /// <summary>
        /// 注销 消息
        /// </summary>
        /// <param UIName="msgEnum">消息</param>
        /// <param UIName="onMsgReceived"> 执行委托 </param>
        /// <returns>true 注销成功 false 该消息下没有该方法</returns>
        public static bool UnRegister(Enum msgType, Action<object> onMsgReceived)
        {
            if (mRegisteredMsgs.ContainsKey(msgType))
            {
                var array = mRegisteredMsgs[msgType].GetInvocationList();
                int index = Array.IndexOf(array, onMsgReceived);
                if (index > 0)
                {
                    mRegisteredMsgs[msgType] -= onMsgReceived;
                    //Debug.Log("注销消息" + msgType + "   number :" + MsgCenter.GetMsgCount(msgType));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 注销 消息的所有委托
        /// </summary>
        /// <param UIName="msgEnum"> 消息体 </param>
        public static void UnRegisterAll(Enum msgType)
        {
            mRegisteredMsgs.Remove(msgType);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param UIName="msgEnum"> 消息体 </param>
        /// <param UIName="data"> 消息参数 </param>
        public static void SendMsg(Enum msgType, object data = null)
        {
            if (mRegisteredMsgs.ContainsKey(msgType))
            {
                mRegisteredMsgs[msgType](data);
            }
            else
            {
                //Debug.LogError("该消息没有注册 msg :" + msgType);
            }
        }

        #endregion

        //#region 接收消息机制 消息等待调用

        ///// <summary>
        ///// 消息体缓存中心
        ///// </summary>
        //public static Dictionary<Enum, List<MsgKeyValue>> messageDic = new Dictionary<Enum, List<MsgKeyValue>>();

        ////消息接收者缓存中心
        ////消息更新时 先从接收者一个个调用
        //public static Dictionary<Enum, Queue<Action<MsgKeyValue>>> waitMsgReceive = new Dictionary<Enum, Queue<Action<MsgKeyValue>>>();

        //public int GetMsgCacheCount
        //{
        //    get { return messageDic.Count; }
        //}

        //public int GetMsgWaitCount
        //{
        //    get { return waitMsgReceive.Count; }
        //}


        //public static void ReceiveMsg(Enum msgType, Action<MsgKeyValue> msgDel,bool isWait = true)
        //{
        //    bool isReceive = MsgReceiveHandle(msgType, msgDel);

        //    if (!isReceive && isWait)
        //    {
        //        if (waitMsgReceive.ContainsKey(msgType))
        //        {
        //            //waitMsgReceive[msgType].Enqueue(msgDel);
        //        }
        //        else
        //        {
        //            Queue<Action<MsgKeyValue>> queue = new Queue<Action<MsgKeyValue>>();
        //            queue.Enqueue(msgDel);
        //            waitMsgReceive.Add(msgType, queue);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 发送消息
        ///// </summary>
        ///// <param UIName="messageType">消息的分类</param>
        ///// <param UIName="kv">键值对(对象)</param>
        //public static void SendMsg(Enum msgType, MsgKeyValue kv)
        //{
        //    bool isSend = MsgSendHandle(msgType, kv);
        //    if (!isSend)
        //    {
        //        if (messageDic.ContainsKey(msgType))
        //        {
        //            messageDic[msgType].Add(kv);
        //        }
        //        else
        //        {
        //            List<MsgKeyValue> list = new List<MsgKeyValue>();
        //            list.Add(kv);
        //            messageDic.Add(msgType, list);
        //        }
        //    }
        //}


        ///// <summary>
        ///// 有消息发送 处理,
        ///// 有消息接收 处理,
        ///// </summary>
        ///// <param UIName="msgEnum"></param>
        //private static bool MsgReceiveHandle(Enum msgType, Action<MsgKeyValue> msgDel)
        //{
        //    if (messageDic.ContainsKey(msgType))
        //    {
        //        List<MsgKeyValue> key_value = messageDic[msgType];
        //        foreach (MsgKeyValue keyValue in key_value)
        //        {
        //            if (waitMsgReceive.ContainsKey(msgType))
        //            {
        //                Queue<Action<MsgKeyValue>> waitQueue = waitMsgReceive[msgType];
        //                while (waitQueue.Count > 0)
        //                {
        //                    waitQueue.Dequeue()(keyValue);
        //                }
        //                waitMsgReceive.Remove(msgType);
        //            }
        //            msgDel(keyValue);
        //        }
        //        messageDic.Remove(msgType);
        //        return true;
        //    }
        //    return false;
        //}

        //private static bool MsgSendHandle(Enum msgType, MsgKeyValue kv)
        //{
        //    if (waitMsgReceive.ContainsKey(msgType))
        //    {
        //        Queue<Action<MsgKeyValue>> waitQueue = waitMsgReceive[msgType];
        //        while (waitQueue.Count > 0)
        //        {
        //            waitQueue.Dequeue()(kv);
        //        }
        //        waitMsgReceive.Remove(msgType);
        //        return true;
        //    }

        //    return false;
        //}

        //#endregion
    }

    /// <summary>
    /// 消息键值对
    /// </summary>
    public class MsgKeyValue
    {
        //键
        private string _Key;

        //值
        private object _Values;

        /*  只读属性  */

        public string Key
        {
            get { return _Key; }
        }

        public object Values
        {
            get { return _Values; }
        }

        public MsgKeyValue(string key, object valueObj)
        {
            _Key = key;
            _Values = valueObj;
        }
    }
}