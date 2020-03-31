/*
*    描述:
*          1.MonoBehaviour扩展类
*
*    开发人: 邓平
*/
using System;
using System.Collections;
using LtFramework.Util;
using UnityEngine;

namespace LtFramework
{
    public class MonoBehaviourEx : MonoEx
    {
        #region MsgSendExe 发送消息

        private MsgOption msgOption = new MsgOption();

        /// <summary>
        /// 消息注册
        /// </summary>
        /// <param UIName="msgType"></param>
        /// <param UIName="msgListener"></param>
        protected void RegisterMsg(Enum msgType, Action<object> msgListener)
        {
            msgOption.RegisterMsg(msgType,msgListener);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param UIName="msgType"></param>
        /// <param UIName="data"></param>
        protected void SendMsg(Enum msgType, object data = null)
        {
            MsgCenter.SendMsg(msgType, data);
        }


        /// <summary>
        /// 注销指定消息
        /// </summary>
        /// <param UIName="msgType"></param>
        protected void UnRegisterMsg(Enum msgType)
        {
            msgOption.UnRegisterMsg(msgType);
        }

        /// <summary>
        /// 注销指定消息
        /// </summary>
        /// <param UIName="msgType"></param>
        /// <param UIName="onMsgReceived"></param>
        protected void UnRegisterMsg(Enum msgType, Action<object> onMsgReceived)
        {
            msgOption.UnRegisterMsg(msgType, onMsgReceived);
        }

        /// <summary>
        /// 注销所有消息
        /// </summary>
        protected void UnRegiserAllMsg()
        {
            msgOption.UnRegiserAllMsg();
        }

        #endregion

        #region Delay

        /// <summary>
        /// 延迟支持 方法
        /// </summary>
        /// <param UIName="seconds">延迟时间</param>
        /// <param UIName="onFinished">等待时间结束后执行的方法</param>
        public void Delay(float seconds, Action onFinished)
        {
            if (seconds <= 0)
            {
                onFinished();
            }
            else
            {
                StartCoroutine(DelayCoroutine(seconds, onFinished));
            }
        }

        /// <summary>
        /// 延迟循环执行函数
        /// </summary>
        /// <param UIName="seconds">延迟时间</param>
        /// <param UIName="onLoopAction">循环函数</param>
        /// <param UIName="times">执行次数</param>
        public void DelayLoop(float seconds, Action onLoopAction,int times = -1)
        {
            StartCoroutine(DelayLoopCoroutine(seconds, onLoopAction,times));
        }

        private IEnumerator DelayLoopCoroutine(float seconds, Action onLoopAction, int times)
        {
            int index = 0;
            while (true)
            {
                yield return new WaitForSeconds(seconds);
                onLoopAction();
                index++;
                if (times > 0 && index >= times)
                {
                    break;
                }
            }
        }

        private IEnumerator DelayCoroutine(float seconds, Action onFinished)
        {
            yield return new WaitForSeconds(seconds);
            onFinished();
        }

        #endregion

        protected override void OnDestroy()
        {
            OnDestroyMono();
            msgOption.UnRegiserAllMsg();
        }

        protected virtual void OnDestroyMono() { }

    }
}
