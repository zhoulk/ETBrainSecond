/*
*    描述:
*          1.
*
*    开发人: 邓平
*/
using System;
using System.Collections.Generic;

namespace LtFramework.Util.Tools
{
    public enum CallBackType
    {
        Null = 0,
    }
    
    public delegate void CallBackEvent(params object[] args);

    public class LtEventCallBackMgr
    {
        private static readonly Dictionary<CallBackType, CallBackEvent> DicMessages = new Dictionary<CallBackType, CallBackEvent>();

        /// <summary>
        /// 添加委托
        /// </summary>
        /// <param UIName="type"></param>
        /// <param UIName="handler"></param>
        public static void AddListener(CallBackType type, CallBackEvent handler)
        {
            
            if (!DicMessages.ContainsKey(type))
            {
                DicMessages.Add(type, null);
            }

            DicMessages[type] += handler;
        }

        /// <summary>
        /// 调用委托 并删除引用
        /// </summary>
        /// <param UIName="type"></param>
        /// <param UIName="args"></param>
        public static void CallListener(CallBackType type, params object[] args)
        {
            CallBackEvent del; //委托

            if (DicMessages.TryGetValue(type, out del))
            {
                if (del != null)
                {
                    Delegate[] delete = del.GetInvocationList();
                    if (delete.Length > 0)
                    {
                        CallBackEvent callBack = delete[0] as CallBackEvent;
                        if (callBack != null)
                        {
                            callBack(args);
                            // ReSharper disable once DelegateSubtraction
                            DicMessages[type] -= callBack;
                        }
                    }
                }
            }
        }
    }
}
