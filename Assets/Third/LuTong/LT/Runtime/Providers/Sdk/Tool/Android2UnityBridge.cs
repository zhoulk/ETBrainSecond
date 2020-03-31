/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Android 桥接类,Android调用Unity的方法
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LT.Net;
using LT.GamepadServer;
using LT.EventDispatcher;

namespace LT.Sdk
{
    /// <summary>
    /// Android 桥接类
    /// </summary>
    internal class Android2UnityBridge : MonoBehaviour
    {
        private SdkTool sdkTool;
        private IPackager packager;
        private bool firstMessage;

        /// <summary>
        /// 设置驱动
        /// </summary>
        /// <param name="sdkTool">sdk公共类</param>
        public void SetDriver(SdkTool sdkTool)
        {
            this.sdkTool = sdkTool;
        }

        /// <summary>
        /// 支付回调
        /// </summary>
        /// <param name="data">支付结果</param>
        public void PaymentBack(string result)
        {
            sdkTool.SetPaymentResult(result);
        }

        /// <summary>
        /// 手柄消息回调
        /// </summary>
        /// <param name="data"></param>
        public void NetKeyEvent(string data)
        {
            if (!firstMessage)
            {
                //当第一次收到消息回调时，先确定新旧版本的sdk解析器
                if (data.IndexOf('-') > 0)
                    packager = new SdkPackager1();
                else
                    packager = new SdkPackager2();

                firstMessage = true;
            }

            IMessage msg = packager.Decode(data);
            App.Make<IEventDispatcher>().Dispatch(new GamepadEventArgs(msg));
        }
    }
}